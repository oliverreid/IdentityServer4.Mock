'use strict';

const DynamoRepo = require('./../dynamo/dynamo-repo.js')

const repo = new DynamoRepo(process.env.EVENT_TEMPLATE_TABLE)

module.exports.list = (event, context, callback) => {
  // fetch all todos from the database
  repo.list((error, results) => {
    // handle potential errors
    if (error) {
      console.error(error);
      callback(null, {
        statusCode: error.statusCode || 501,
        headers: { 'Content-Type': 'text/plain' },
        body: 'Couldn\'t fetch the events.',
      });
      return;
    }

    // create a response
    const response = {
      statusCode: 200,
      body: JSON.stringify(results),
    };
    callback(null, response);
  });
};
