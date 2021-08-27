<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/199043012/19.1.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T828553)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Dashboard Client-Side Application (React)

This example is a ready-to-use client React application with the DevExpress Dashboard component.

> **Documentation**: [Client-Side Configuration (React)](https://docs.devexpress.com/Dashboard/400683)

The example uses a modular approach that based on the client-server model. You need a server (backend) project and a client (frontend) application that includes all the necessary styles, scripts and HTML-templates. Note that the script version on the client should match with libraries version on the server up to a minor version.

The server project hosts on ```https://demos.devexpress.com/services/dashboard/api```.

The [JS](JS) folder contains a client application.

## Quick Start

1. Open the **JS** folder. In the command prompt, download and install npm packages used in the application:

    ```
    npm install
    ```

2. In the same folder, run the following command to compile and run the application:

    ```bash
    npm start
    ```

3. Open ```http://localhost:3000/``` in your browser to see the result. The HTML JavaScript Dashboard displays the dashboard stored on the preconfigured server (```https://demos.devexpress.com/services/dashboard/api```).

## See Also

Documentation:
- [Client-Side Configuration (React)](https://docs.devexpress.com/Dashboard/400683)
- [HTML JavaScript Dashboard Control](https://docs.devexpress.com/Dashboard/119108/)

Examples:
- [Dashboard Client-Side Application (Angular)](https://github.com/DevExpress-Examples/dashboard-angular-app)
- [Dashboard Client-Side Application (Vue)](https://github.com/DevExpress-Examples/dashboard-vue-app)
