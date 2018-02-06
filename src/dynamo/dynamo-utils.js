const reservedWords = require('./dynamo-reserved-words')

module.exports = {
  buildUpdateObject: function(patch) {
    var varCount = 0;
    const nextVar = () => `:v${++varCount}`
    const serializePatch = (patch, path, init) => {
      path = path || ""
      init = init ||  {
        updates: [],
        removes: []
      };
      return Object.keys(patch).reduce((agg, next) => {
        const val = patch[next];
        const prop = `${path}${path.length > 0 ? "." : ""}${next}`
        if(typeof val === 'object' && !Array.isArray(val) && !!val) {
          return serializePatch(val, prop, agg)
        } else {
          const thisVar = nextVar();
          var key = prop;
          if(reservedWords.indexOf(key.toUpperCase()) >= 0) {
            key = `#alias_${key}`
            agg.ExpressionAttributeNames = agg.ExpressionAttributeNames || {}
            agg.ExpressionAttributeNames[key] = prop;
          }
          if(val) {
            agg.updates.push(`${key}=${thisVar}`)
            agg.ExpressionAttributeValues[thisVar] = val
          } else {
            agg.removes.push(key)
          }
          return agg;
        }
      }, init)
    }
    var retval = serializePatch(patch);
    if(retval.updates.length){
      retval.UpdateExpression = retval.updates.reduce((agg,n) => agg + n + ", ", "set ")
      retval.UpdateExpression = retval.UpdateExpression.substring(0, retval.UpdateExpression.length - 2)
    }
    if(retval.removes.length){
      retval.UpdateExpression += (retval.UpdateExpression || "") + retval.removes.reduce((agg,n) => agg + n + ", ", " remove ")
      retval.UpdateExpression = retval.UpdateExpression.substring(0, retval.UpdateExpression.length - 2)
    }
    delete retval.updates
    delete retval.removes
    return retval;
  }
}
