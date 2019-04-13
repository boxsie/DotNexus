var dotnexus =
(window["webpackJsonpdotnexus"] = window["webpackJsonpdotnexus"] || []).push([["main"],{

/***/ "./Resources/Scripts/main.js":
/*!***********************************!*\
  !*** ./Resources/Scripts/main.js ***!
  \***********************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"default\", function() { return Layout; });\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! vue */ \"./node_modules/vue/dist/vue.js\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var bootstrap__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! bootstrap */ \"./node_modules/bootstrap/dist/js/bootstrap.js\");\n/* harmony import */ var bootstrap__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(bootstrap__WEBPACK_IMPORTED_MODULE_2__);\n/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! electron */ \"electron\");\n/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(electron__WEBPACK_IMPORTED_MODULE_3__);\n/* harmony import */ var _Styles_layout_scss__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../Styles/layout.scss */ \"./Resources/Styles/layout.scss\");\n/* harmony import */ var _Styles_layout_scss__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_Styles_layout_scss__WEBPACK_IMPORTED_MODULE_4__);\n﻿\r\n\r\n\r\n\r\n\r\n\r\n\r\nclass Layout {\r\n    constructor() {\r\n        window.vm = new vue__WEBPACK_IMPORTED_MODULE_1___default.a({\r\n            el: '#layout',\r\n            data: {\r\n            },\r\n            mounted() {\r\n                electron__WEBPACK_IMPORTED_MODULE_3__[\"ipcRenderer\"].send('notification', 'ping');\r\n            }\r\n        });\r\n    }\r\n}\n\n//# sourceURL=webpack://dotnexus/./Resources/Scripts/main.js?");

/***/ }),

/***/ "./Resources/Styles/layout.scss":
/*!**************************************!*\
  !*** ./Resources/Styles/layout.scss ***!
  \**************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("// removed by extract-text-webpack-plugin\n\n//# sourceURL=webpack://dotnexus/./Resources/Styles/layout.scss?");

/***/ }),

/***/ "electron":
/*!***************************!*\
  !*** external "electron" ***!
  \***************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("module.exports = require(\"electron\");\n\n//# sourceURL=webpack://dotnexus/external_%22electron%22?");

/***/ })

},[["./Resources/Scripts/main.js","runtime","vendor"]]]);