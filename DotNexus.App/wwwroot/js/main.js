var dotnexus =
(window["webpackJsonpdotnexus"] = window["webpackJsonpdotnexus"] || []).push([["main"],{

/***/ "./Resources/Images/logo.png":
/*!***********************************!*\
  !*** ./Resources/Images/logo.png ***!
  \***********************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

eval("module.exports = __webpack_require__.p + \"img/logo.png\";\n\n//# sourceURL=webpack://dotnexus/./Resources/Images/logo.png?");

/***/ }),

/***/ "./Resources/Scripts/layout.js":
/*!*************************************!*\
  !*** ./Resources/Scripts/layout.js ***!
  \*************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! vue */ \"./node_modules/vue/dist/vue.js\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var _aspnet_signalr__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @aspnet/signalr */ \"./node_modules/@aspnet/signalr/dist/esm/index.js\");\n/* harmony import */ var _dicebear_avatars__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @dicebear/avatars */ \"./node_modules/@dicebear/avatars/lib/index.js\");\n/* harmony import */ var _dicebear_avatars__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dicebear_avatars__WEBPACK_IMPORTED_MODULE_3__);\n/* harmony import */ var _dicebear_avatars_identicon_sprites__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @dicebear/avatars-identicon-sprites */ \"./node_modules/@dicebear/avatars-identicon-sprites/lib/sprites.js\");\n/* harmony import */ var _dicebear_avatars_identicon_sprites__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dicebear_avatars_identicon_sprites__WEBPACK_IMPORTED_MODULE_4__);\n\r\n\r\n\r\n\r\n\r\n\r\n/* harmony default export */ __webpack_exports__[\"default\"] = ({\r\n    install(Vue, options) {\r\n        Vue.prototype.$layout = new Vue({\r\n            el: '#layout',\r\n            data: {\r\n                latestBlock: {},\r\n                identiconSvg: ''\r\n            },\r\n            methods: {\r\n                parseBlockChannel(channel) {\r\n                    return channel;\r\n                },\r\n                parseTxType(txType) {\r\n                    return txType;\r\n                },\r\n                parseBytes(bytes) {\r\n                    const sizes = ['B', 'KB', 'MB', 'GB', 'TB'];\r\n                    if (bytes === 0) return '0 Byte';\r\n                    const i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));\r\n                    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];\r\n                }\r\n            },\r\n            mounted() {\r\n                jquery__WEBPACK_IMPORTED_MODULE_0___default()('.toast').toast({ delay: 5000 });\r\n\r\n                this.connection = new _aspnet_signalr__WEBPACK_IMPORTED_MODULE_2__[\"HubConnectionBuilder\"]()\r\n                    .configureLogging(_aspnet_signalr__WEBPACK_IMPORTED_MODULE_2__[\"LogLevel\"].Information)\r\n                    .withUrl('/blockchainhub')\r\n                    .build();\r\n\r\n                this.connection.start()\r\n                    .then(() => {\r\n                        this.connection.on('newBlockNotify', (block) => {\r\n                            this.latestBlock = block;\r\n                            this.$emit('new-block-notify', this.latestBlock);\r\n                            jquery__WEBPACK_IMPORTED_MODULE_0___default()('#newBlockToast').toast('show');\r\n                        });\r\n                    });\r\n\r\n                if (options.userGenesis) {\r\n                    const avatars = new _dicebear_avatars__WEBPACK_IMPORTED_MODULE_3___default.a(_dicebear_avatars_identicon_sprites__WEBPACK_IMPORTED_MODULE_4___default.a);\r\n                    const svg = avatars.create(options.userGenesis);\r\n                    this.identiconSvg = svg;\r\n                }\r\n            }\r\n        });\r\n    }\r\n});\n\n//# sourceURL=webpack://dotnexus/./Resources/Scripts/layout.js?");

/***/ }),

/***/ "./Resources/Scripts/main.js":
/*!***********************************!*\
  !*** ./Resources/Scripts/main.js ***!
  \***********************************/
/*! exports provided: Main */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"Main\", function() { return Main; });\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! vue */ \"./node_modules/vue/dist/vue.js\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var bootstrap__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! bootstrap */ \"./node_modules/bootstrap/dist/js/bootstrap.js\");\n/* harmony import */ var bootstrap__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(bootstrap__WEBPACK_IMPORTED_MODULE_2__);\n/* harmony import */ var _aspnet_signalr__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @aspnet/signalr */ \"./node_modules/@aspnet/signalr/dist/esm/index.js\");\n/* harmony import */ var _layout__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./layout */ \"./Resources/Scripts/layout.js\");\n/* harmony import */ var _Styles_layout_scss__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../Styles/layout.scss */ \"./Resources/Styles/layout.scss\");\n/* harmony import */ var _Styles_layout_scss__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_Styles_layout_scss__WEBPACK_IMPORTED_MODULE_5__);\n/* harmony import */ var _Images_logo_png__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../Images/logo.png */ \"./Resources/Images/logo.png\");\n/* harmony import */ var _Images_logo_png__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_Images_logo_png__WEBPACK_IMPORTED_MODULE_6__);\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nclass Main {\r\n    constructor(options) {\r\n        vue__WEBPACK_IMPORTED_MODULE_1___default.a.use(_layout__WEBPACK_IMPORTED_MODULE_4__[\"default\"], options);\r\n    }\r\n}\n\n//# sourceURL=webpack://dotnexus/./Resources/Scripts/main.js?");

/***/ }),

/***/ "./Resources/Styles/layout.scss":
/*!**************************************!*\
  !*** ./Resources/Styles/layout.scss ***!
  \**************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("// removed by extract-text-webpack-plugin\n\n//# sourceURL=webpack://dotnexus/./Resources/Styles/layout.scss?");

/***/ }),

/***/ 0:
/*!************************!*\
  !*** crypto (ignored) ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("/* (ignored) */\n\n//# sourceURL=webpack://dotnexus/crypto_(ignored)?");

/***/ })

},[["./Resources/Scripts/main.js","runtime","vendor"]]]);