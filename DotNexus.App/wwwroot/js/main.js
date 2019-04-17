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
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! vue */ \"./node_modules/vue/dist/vue.js\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var _aspnet_signalr__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @aspnet/signalr */ \"./node_modules/@aspnet/signalr/dist/esm/index.js\");\n\n\n\n\n/* harmony default export */ __webpack_exports__[\"default\"] = ({\n    install(Vue, options) {\n        Vue.prototype.$layout = new Vue({\n            el: '#layout',\n            data: {\n                latestBlock: {}\n            },\n            methods: {\n                parseBlockChannel(channel) {\n                    return channel;\n                },\n                parseTxType(txType) {\n                    return txType;\n                },\n                parseBytes(bytes) {\n                    const sizes = ['B', 'KB', 'MB', 'GB', 'TB'];\n                    if (bytes === 0) return '0 Byte';\n                    const i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));\n                    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];\n                }\n            },\n            mounted() {\n                jquery__WEBPACK_IMPORTED_MODULE_0___default()('.toast').toast({ delay: 5000 });\n\n                this.connection = new _aspnet_signalr__WEBPACK_IMPORTED_MODULE_2__[\"HubConnectionBuilder\"]()\n                    .configureLogging(_aspnet_signalr__WEBPACK_IMPORTED_MODULE_2__[\"LogLevel\"].Information)\n                    .withUrl('/blockchainhub')\n                    .build();\n\n                this.connection.start()\n                    .then(() => {\n                        this.connection.on('newBlockNotify', (block) => {\n                            this.latestBlock = block;\n                            this.$emit('new-block-notify', this.latestBlock);\n                            jquery__WEBPACK_IMPORTED_MODULE_0___default()('#newBlockToast').toast('show');\n                        });\n                    });\n            }\n        });\n    }\n});\n\n//# sourceURL=webpack://dotnexus/./Resources/Scripts/layout.js?");

/***/ }),

/***/ "./Resources/Scripts/main.js":
/*!***********************************!*\
  !*** ./Resources/Scripts/main.js ***!
  \***********************************/
/*! exports provided: Main */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"Main\", function() { return Main; });\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! vue */ \"./node_modules/vue/dist/vue.js\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var bootstrap__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! bootstrap */ \"./node_modules/bootstrap/dist/js/bootstrap.js\");\n/* harmony import */ var bootstrap__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(bootstrap__WEBPACK_IMPORTED_MODULE_2__);\n/* harmony import */ var _aspnet_signalr__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @aspnet/signalr */ \"./node_modules/@aspnet/signalr/dist/esm/index.js\");\n/* harmony import */ var _layout__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./layout */ \"./Resources/Scripts/layout.js\");\n/* harmony import */ var _Styles_layout_scss__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../Styles/layout.scss */ \"./Resources/Styles/layout.scss\");\n/* harmony import */ var _Styles_layout_scss__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_Styles_layout_scss__WEBPACK_IMPORTED_MODULE_5__);\n/* harmony import */ var _Images_logo_png__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../Images/logo.png */ \"./Resources/Images/logo.png\");\n/* harmony import */ var _Images_logo_png__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_Images_logo_png__WEBPACK_IMPORTED_MODULE_6__);\n\n\n\n\n\n\n\n\n\n\nclass Main {\n    constructor(options) {\n        vue__WEBPACK_IMPORTED_MODULE_1___default.a.use(_layout__WEBPACK_IMPORTED_MODULE_4__[\"default\"], options);\n\n        window.vm = new vue__WEBPACK_IMPORTED_MODULE_1___default.a({\n            el: '#header',\n            mounted() {\n            }\n        });\n    }\n}\n\n//# sourceURL=webpack://dotnexus/./Resources/Scripts/main.js?");

/***/ }),

/***/ "./Resources/Styles/layout.scss":
/*!**************************************!*\
  !*** ./Resources/Styles/layout.scss ***!
  \**************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("// removed by extract-text-webpack-plugin\n\n//# sourceURL=webpack://dotnexus/./Resources/Styles/layout.scss?");

/***/ })

},[["./Resources/Scripts/main.js","runtime","vendor"]]]);