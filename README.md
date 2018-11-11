[![NPM Version][npm-image]][npm-url]
[![Build Status][travis-image]][travis-url]
[![Downloads Stats][npm-downloads]][npm-url]

---

# Sage | Pace

<img align="right" width="100" height="100" src="https://avatars1.githubusercontent.com/u/20365551?s=400&u=e500e44c444dc1edd386184520cef4cbb79c448c&v=4">

> TODO: Convert Framework model from event based programming to: MVVM (Model-View-ViewModel)


 

Sage is a theoretical design implementation to resolve the needs of an existing physical system as they adapt into paper-less data record keeping. Also, My first attempt at using WPF to control the front-end within the .NET framework. Using XAML for the interface was a different experience yet very similar to embedded css in html markup. Once I get the hang of data binding with a View Model, things will be stranger yet!

---




```sh
The program was designed with the goal of utilizing Oracle SQL knowledge attained from the CCS014 ITEDBASE-01 class of 2018
```

## Installation

Windows:

```sh
Install .Net Framework 4.7.2.
Install Oracle Database XE 11gRE3.
Locate Data Pump from within SQL Devevloper and select View > DBA > Import on the export dump file provided.
Install Oracle Data Access Client Managed Driver.
Then..
Download file from releases & extract
Run Sage.exe
```

*Note: The connection string target is given by the app.config file included in the program.

## Logical Design

[![Ldesign](https://github.com/Aroueterra/Sage-Pace/blob/master/graphics/Logical.png)]()

You will notice that Author Master and Genre Master were broken down from their place in the Book Table, this is to allow any book to possess more than a single author, or more than a single genre. One tag may exist as multiple entries in the other table, thus, not their ID but an auto incremental dummy column was devised to allow insertion of records without constraints.
This was modeled after the data of the client; thus, the scope may be small or unexpected. Until their model changes, the data dictionary will adapt to its needs.
A part of their design is to only allow borrowing of books in single intervals. Because of this, each created order will only reduce the quantity of a record by 1 per transaction, and increase the quantity by 1 for each resolved transaction

## Relational Design

[![Rdesign](https://github.com/Aroueterra/Sage-Pace/blob/master/graphics/Relational.png)]()




## Feeding the Excel Document

To batch import data into the program, click the import/export tab and select your Table target from the combo box.

Then, click the export button and select the SageLedger.xlsx file provided. Certain columns are omitted by default and the entered data needs to be carefully sanitized.

_For more examples and usage, please refer to the [Wiki][wiki]._


## Keeping track of inventory

[![Inventory screen](https://github.com/Aroueterra/Sage-Pace/blob/master/graphics/Inventory.png)]()

## Manage quantity reductions and overdue balances

[![Orders screen](https://github.com/Aroueterra/Sage-Pace/blob/master/graphics/Orders.png)]()

## CRUD Manipulation

[![select screen](https://github.com/Aroueterra/Sage-Pace/blob/master/graphics/side.png)]()


```sh
TBD
```

## Release History


* 0.0.1
    * Initial release

## Meta

August Bryan N. Florese – [@Aroueterra](https://www.facebook.com/Aroueterra) – aroueterra@gmail.com

Distributed under the Mit license. See ``LICENSE`` for more information.

[https://github.com/Aroueterra/](https://github.com/Aroueterra/)

## Contributing

1. Fork it 
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request

<!-- Markdown link & img dfn's -->
[npm-image]: https://img.shields.io/npm/v/datadog-metrics.svg?style=flat-square
[npm-url]: https://npmjs.org/package/datadog-metrics
[npm-downloads]: https://img.shields.io/npm/dm/datadog-metrics.svg?style=flat-square
[travis-image]: https://img.shields.io/travis/dbader/node-datadog-metrics/master.svg?style=flat-square
[travis-url]: https://travis-ci.org/dbader/node-datadog-metrics
[wiki]: https://github.com/yourname/yourproject/wiki


