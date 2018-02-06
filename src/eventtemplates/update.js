'use strict';

const AWS = require('aws-sdk'); // eslint-disable-line import/no-extraneous-dependencies
const utils = require('./../dynamo/dynamo-utils.js')

const dynamoDb = new AWS.DynamoDB.DocumentClient();

module.exports.update = (event, context, callback) => {
  const timestamp = new Date().getTime();
  const data = JSON.parse(event.body);

  delete data.name
  delete data.createdAt
  delete data.published
  delete data.id

  const params = {
    TableName: process.env.EVENT_TEMPLATE_TABLE,
    Key: {
      id: event.pathParameters.id,
    },
    ReturnValues: 'ALL_NEW',
  };

  var updateObject = Object.assign({}, params, utils.buildUpdateObject(data))

  // update the todo in the database
  dynamoDb.update(updateObject, (error, result) => {
    // handle potential errors
    if (error) {
      console.error(error);
      callback(null, {
        statusCode: error.statusCode || 501,
        headers: { 'Content-Type': 'text/plain' },
        body: 'Couldn\'t fetch the event.',
      });
      return;
    }

    // create a response
    const response = {
      statusCode: 200,
      body: JSON.stringify(result.Attributes),
    };
    callback(null, response);
  });
};
