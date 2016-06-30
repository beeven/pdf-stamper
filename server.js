var express = require("express"),
    app = express(),
    bodyParser = require("body-parser"),
    pdfGenerator = require("./pdf-generator"),
    fs = require("fs");


app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended: false}));



/*
{
    content: htmlContentToPrint,
    pageSize: {
        format: 'A4',
        orientation: 'landscape',
        margin: '1cm'
    }
}
*/
app.post("/",function(req,res){
    var content = req.body.content;
    var pageSize = req.body.pageSize;
    if(!content) {
        return res.status(500).end("no content to convert");
    }
    pdfGenerator.generatePDF(content,pageSize)
        .then((filename)=>{
            res.attachment(filename);
            res.sendFile(filename,function(err){
                if(err) {
                    console.error("send file error:",err);
                }
                fs.unlinkSync(filename);
            });
        })
        .catch(function(err){
            console.error("generate pdf error:",err);
            res.status(500).json(err);
        });
});

app.get("/",function(req,res){
    res.end("Hello from pdf-generator");
});



pdfGenerator.init().then(()=>{
    app.listen(80);
    console.log("server listening on 80");
},(err)=>{
    console.error("Init error:",err.toString());
});
