'use strict';

const schemas = require('./../schemas.js')
const uuid = require('uuid');
const DynamoRepo = require('./../dynamo/dynamo-repo.js')

const repo = new DynamoRepo(process.env.EVENT_TEMPLATE_TABLE)

module.exports.create = (event, context, callback) => {
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

    const item = Object.assign({}, data, {
      id: uuid.v1(),
      published: false,
      createdAt: timestamp,
      updatedAt: timestamp,
    });

    // write the todo to the database
    repo.create(item, (error) => {
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
        body: JSON.stringify(item),
      };
      callback(null, response);
    });
  })
};
