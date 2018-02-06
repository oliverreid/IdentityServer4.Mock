'use strict';

const schemas = require('./../schemas.js')
const uuid = require('uuid');
const AWS = require('aws-sdk'); // eslint-disable-line import/no-extraneous-dependencies

const dynamoDb = new AWS.DynamoDB.DocumentClient();

module.exports.save = (event, context, callback) => {
  const timestamp = new Date().getTime();
  const data = JSON.parse(event.body);

  schemas.validate(schemas.schemas.eventTempalteRequest, data, (err) => {
    if(err) {
      console.error('Validation Failed');
      callback(null, {
        statusCode: 400,
        headers: { 'Content-Type': 'text/plain' },
        body: err,
      });
      return;
    }

    const params = {
      TableName: process.env.EVENT_TEMPLATE_TABLE,
      Item: {
        id: uuid.v1(),
        name: data.name,
        published: false,
        createdAt: timestamp,
        updatedAt: timestamp,
      },
    };

    // write the todo to the database
    dynamoDb.put(params, (error) => {
      // handle potential errors
      if (error) {
        console.error(error);
        callback(null, {
          statusCode: error.statusCode || 501,
          headers: { 'Content-Type': 'text/plain' },
          body: 'Couldn\'t create event.',
        });
        return;
      }

      // create a response
      const response = {
        statusCode: 200,
        body: JSON.stringify(params.Item),
      };
      callback(null, response);
    });
  })
};
