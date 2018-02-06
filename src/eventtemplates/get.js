'use strict';


const DynamoRepo = require('./../dynamo/dynamo-repo.js')

const repo = new DynamoRepo(process.env.EVENT_TEMPLATE_TABLE)

module.exports.get = (event, context, callback) => {

  // fetch todo from the database
  repo.get({
    id: event.pathParameters.id,
  }, (error, result) => {
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
      body: JSON.stringify(result),
    };
    callback(null, response);
  });
};
