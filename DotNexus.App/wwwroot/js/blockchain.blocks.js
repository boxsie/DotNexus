var dotnexus =
(window["webpackJsonpdotnexus"] = window["webpackJsonpdotnexus"] || []).push([["blockchain.blocks"],{

/***/ "./Resources/Scripts/blockchain.blocks.js":
/*!************************************************!*\
  !*** ./Resources/Scripts/blockchain.blocks.js ***!
  \************************************************/
/*! exports provided: BlockchainBlocks */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"BlockchainBlocks\", function() { return BlockchainBlocks; });\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! vue */ \"./node_modules/vue/dist/vue.js\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_1__);\n\n\n\nclass BlockchainBlocks {\n    constructor(options) {\n        window.vm = new vue__WEBPACK_IMPORTED_MODULE_1___default.a({\n            el: '#body',\n            data: {\n                latestBlocks: options.latestBlocks.reverse()\n            },\n            mounted() {\n                this.$layout.$on('new-block-notify', (block) => {\n                    this.latestBlocks.splice(0, 0, block);\n                    this.latestBlocks.pop();\n                });\n            }\n        });\n    }\n}\n\n//# sourceURL=webpack://dotnexus/./Resources/Scripts/blockchain.blocks.js?");

/***/ })

},[["./Resources/Scripts/blockchain.blocks.js","runtime","vendor"]]]);