var pdfgen = require("../pdf-generator");
var fs = require("fs");

var content = fs.readFileSync(__dirname+"/printTemp.html").toString();

pdfgen.init().then(()=>{
    return pdfgen.generatePDF(content)
}).then((filename)=>{
    console.log("Output:",filename);
    process.exit();
}).catch((err)=>{
    console.error("error:",err);
})
