const reservedWords = require('./dynamo-reserved-words')

module.exports = {
  buildUpdateObject: function(patch) {
    var varCount = 0;
    const nextVar = () => `:v${++varCount}`
    const serializePatch = (patch, path, init) => {
      path = path || ""
      init = init ||  {
        UpdateExpression: "set ",
        ExpressionAttributeValues: {},
        ExpressionAttributeNames: {}
      };
      return Object.keys(patch).reduce((agg, next) => {
        const val = patch[next];
        const prop = `${path}${path.length > 0 ? "." : ""}${next}`
        if(typeof val === 'object' && !Array.isArray(val)) {
          return serializePatch(val, prop, agg)
        } else {
          const thisVar = nextVar();
          var key = prop;
          if(reservedWords.indexOf(key.toUpperCase()) >= 0) {
            key = `#alias_${key}`
            agg.ExpressionAttributeNames[key] = prop;
          }
          agg.UpdateExpression = agg.UpdateExpression + `${key}=${thisVar}, `
          agg.ExpressionAttributeValues[thisVar] = val
          return agg;
        }
      }, init)
    }
    var retval = serializePatch(patch);
    retval.UpdateExpression = retval.UpdateExpression.substring(0, retval.UpdateExpression.length - 2)
    return retval;
  }
}
