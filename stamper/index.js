var path = require("path"),
    edge = require("edge"),
    stamp = edge.func({
        source: path.join(__dirname, "Stamper.cs"),
        references: [path.join(__dirname,"itextsharp.dll")]
    });

exports.stamp = function(filename, callback){
    if(typeof(callback) === 'function') {
        stamp(filename,callback);
    } else {
        return new Promise(function(resolve,reject){
            stamp(filename,function(err,result){
                //console.log("done signing in js");
                if(err) {
                    return reject(err);
                }
                resolve(result);
            });
        });
    }
};
