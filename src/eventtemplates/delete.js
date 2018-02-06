
'use strict';

const DynamoRepo = require('./../dynamo/dynamo-repo.js')

const repo = new DynamoRepo(process.env.EVENT_TEMPLATE_TABLE)

module.exports.delete = (event, context, callback) => {

  // delete the todo from the database
  repo.delete({
    id: event.pathParameters.id,
  }, (error) => {
    // handle potential errors
    if (error){
      console.error(error);
      callback(null, {
        statusCode: error.statusCode || 501,
        headers: { 'Content-Type': 'text/plain' },
        body: 'Couldn\'t remove the event',
      });
      return;
    }

    // create a response
    const response = {
      statusCode: 200,
      body: JSON.stringify({}),
    };
    callback(null, response);
  });

}
