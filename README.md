pdf-stamper
====

A server to convert html to pdf and sign it with given certificate


Usage
-----
You can pull the prebuilt image from docker or build it from source.

### Pull image from docker repository
```bash
docker pull beeven/pdf-stamper
```

### Build from source
Prerequisite packages:
* mono
* python
* pkg-config
* python
* libfontconfig
* nodejs

Install all prerequisite packages then run ```npm install``` to build.


API
----

### Generate pdf without signature
**PATH:** /
**METHOD:** POST
**ARGUMENTS:**
* *content:* html content to convert
* *pageSize:* page size configuration
    * *format:* paper size, `'A4', `'A5', `'B5'`, etc
    * *orientation:* `'landscape'` or `'portrait'`
    * *margin:* bleed, `1cm` or specified each edge ```{top:'1cm',bottom:'3cm',left:'1cm',right:'1cm'}```

Example: see example directory.
