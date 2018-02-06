const AWS = require('aws-sdk')
const dynamoDb = new AWS.DynamoDB.DocumentClient();
const utils = require('./../dynamo/dynamo-utils.js')

class DynamoRepository {
  constructor(table){
    this.table = table;
  }
  create(item, cb){
    const params = {
      TableName: this.table,
      Item: item
    };

    dynamoDb.put(params, (error) => {
      if (error) {
        cb(error);
      } else {
        cb();
      }
    });
  }
  delete(key, cb){
    const params = {
      TableName: this.table,
      Key: key
    };

    // delete the todo from the database
    dynamoDb.delete(params, (error) => {
      // handle potential errors
      if (error){
        cb(error)
      } else {
        cb()
      }
    });
  }
  get(key, cb){
    const params = {
      TableName: this.table,
      Key: key
    };

    // fetch todo from the database
    dynamoDb.get(params, (error, result) => {
      // handle potential errors
      if (error) {
        cb(err)
      } else {
        cb(null, result.Item)
      }
    });
  }
  list(cb){
    const params = {
      TableName: this.table
    };

    dynamoDb.scan(params, (error, result) => {
      // handle potential errors
      if (error) {
        cb(err)
      } else {
        cb(null, result.Items)
      }
    });
  }
  update(key, data, cb){
    const params = {
      TableName:this.table,
      Key: key,
      ReturnValues: 'ALL_NEW',
    };

    var updateObject = Object.assign({}, params, utils.buildUpdateObject(data))

    // update the todo in the database
    dynamoDb.update(updateObject, (error, result) => {
      // handle potential errors
      if (error) {
        cb(err)
      } else {
        cb(null, result.Attributes)
      }
    });
  }
}

module.exports = DynamoRepository
