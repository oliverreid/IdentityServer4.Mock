var Ajv = require('ajv');

var eventTemplateRequest = {
  "$id": "http://dostuff.now/schemas/event-template.json",
  "type": "object",
  "properties": {
    "name": { "type": "string" }
  },
  "required": [ "name" ]
};

var ajv = new Ajv({schemas: [eventTemplateRequest]});

module.exports = {
  validate: function(name, data, cb){
    var validate = ajv.getSchema(name);
    var valid = validate(data);
    if(!valid) {
      cb(valid.errorsText())
    } else {
      cb()
    }
  },
  schemas: {
    eventTempalteRequest: eventTemplateRequest["$id"]
  }
}
