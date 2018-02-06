'use strict';


const DynamoRepo = require('./../dynamo/dynamo-repo.js')

const repo = new DynamoRepo(process.env.EVENT_TEMPLATE_TABLE)

module.exports.update = (event, context, callback) => {
  const timestamp = new Date().getTime();
  const data = JSON.parse(event.body);

  delete data.name
  delete data.createdAt
  delete data.published
  delete data.id

  // update the todo in the database
  repo.update({
    id: event.pathParameters.id,
  }, data, (error, result) => {
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
