var should = require("should");
var fs = require("fs");


describe("pdf-generator",function(){
    var pdfgen = require("../pdf-generator");
    before(function(done){
        pdfgen.init().then(function(){done();})
    });

    it("should generate a pdf to /tmp and the file size > 0",function(){
        return pdfgen.generatePDF("<html><body><div>Hello World!</div></body></html>")
                .should.be.finally.a.String()
                .and.match(function(it){
                    console.log(it);
                    var stat = fs.statSync(it);
                    stat.isFile().should.be.true();
                    stat.size.should.be.above(0);
                });
    });
});
