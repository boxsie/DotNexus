var dotnexus =
(window["webpackJsonpdotnexus"] = window["webpackJsonpdotnexus"] || []).push([["blockchain.genesis"],{

/***/ "./Resources/Markup/data-table-vue.html":
/*!**********************************************!*\
  !*** ./Resources/Markup/data-table-vue.html ***!
  \**********************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("module.exports = \"<div class=\\\"dt-wrapper\\\">\\r\\n    <div class=\\\"row\\\">\\r\\n        <div class=\\\"col text-toggle\\\">\\r\\n            <span v-for=\\\"(f, i) in dtOptions.filters\\\">\\r\\n                <a href=\\\"#\\\" v-on:click.prevent.stop=\\\"changeFilter(i)\\\" :class=\\\"getFilterClass(i)\\\">{{ f.name }}</a>\\r\\n                <span v-if=\\\"i < dtOptions.filters.length - 1\\\" class=\\\"toggle-seperator fa fa-ellipsis-v\\\"></span>\\r\\n            </span>\\r\\n        </div>\\r\\n    \\r\\n        <div :class=\\\"filterClass\\\" v-if=\\\"showUserFilter\\\" v-cloak>\\r\\n            <slot name=\\\"dt-criteria\\\" :dtCriteria=\\\"dtCriteria\\\"></slot>\\r\\n        </div>\\r\\n    </div>\\r\\n    \\r\\n    <div class=\\\"data-table\\\">\\r\\n        <div class=\\\"dt-body\\\">\\r\\n            <preloader v-if=\\\"isLoading\\\"></preloader>\\r\\n\\r\\n            <slot name=\\\"dt\\\" :dt=\\\"dt\\\">\\r\\n                <div :class=\\\"headerRowClass\\\">\\r\\n                    <div v-if=\\\"dtOptions.showRowIndex\\\" class=\\\"index ml-4\\\">\\r\\n                    </div>\\r\\n                    <div v-for=\\\"c in columns\\\" :class=\\\"parseColClass(c, 'dt-head')\\\" v-html=\\\"c.header\\\"></div>\\r\\n                </div>\\r\\n                \\r\\n                <div v-if=\\\"tableData.totalItems === 0\\\" class=\\\"text-center\\\">\\r\\n                    <hr/>\\r\\n                    There are no results\\r\\n                    <hr/>\\r\\n                </div>\\r\\n\\r\\n                <div v-else v-for=\\\"(data, i) in tableData.pageItems\\\" :class=\\\"parseRowClass(i)\\\" v-on:click=\\\"$emit('row-click', data)\\\">\\r\\n                    <div v-if=\\\"dtOptions.showRowIndex\\\" class=\\\"index\\\">\\r\\n                        <strong>{{ ((criteria.page - 1) * criteria.length) + (i + 1)}}</strong>\\r\\n                    </div>\\r\\n                    <div v-for=\\\"c in columns\\\" :class=\\\"parseColClass(c, 'dt-col')\\\" v-html=\\\"c.render(data[c.key], data, i)\\\"></div>\\r\\n                </div>\\r\\n            </slot>\\r\\n        </div>\\r\\n\\r\\n        <div class=\\\"row dt-controls\\\">\\r\\n            <div class=\\\"col-md-5\\\">\\r\\n                <div class=\\\"form-group\\\">\\r\\n                    <label for=\\\"pageLength\\\">Showing </label>\\r\\n                    <select id=\\\"pageLength\\\" class=\\\"form-control\\\" style=\\\"width: auto;\\\" v-model=\\\"criteria.length\\\" v-on:change=\\\"changeLength()\\\">\\r\\n                        <option v-for=\\\"pl in dtOptions.availableLengths\\\" v-bind:value=\\\"pl\\\">{{pl}}</option>\\r\\n                    </select>\\r\\n                    <span class=\\\"total-results\\\">of {{ tableData.totalItems }}</span>\\r\\n                </div>\\r\\n            </div>\\r\\n            <div class=\\\"col\\\" v-if=\\\"tableData.totalItems > criteria.length\\\">\\r\\n                <ul class=\\\"paging\\\">\\r\\n                    <li class=\\\"d-none d-sm-inline\\\"><a href=\\\"#\\\" :class=\\\"criteria.page > 1 ? 'enabled' : 'disabled'\\\" v-on:click.prevent.stop=\\\"changePage(criteria.page - 1)\\\">Prev</a></li>\\r\\n                    \\r\\n                    <li v-for=\\\"pn in pageNumbers\\\">\\r\\n                        <a href=\\\"#\\\" :class=\\\"pageNumberClass(pn)\\\" v-on:click.prevent.stop=\\\"changePage(pn)\\\">{{pn}}</a>\\r\\n                    </li>\\r\\n\\r\\n                    <li class=\\\"d-none d-sm-inline\\\"><a href=\\\"#\\\" :class=\\\"criteria.page < pageCount ? 'enabled' : 'disabled'\\\" v-on:click.prevent.stop=\\\"changePage(criteria.page + 1)\\\">Next</a></li>\\r\\n                </ul>\\r\\n            </div>\\r\\n        </div>\\r\\n    </div>\\r\\n</div>\";\n\n//# sourceURL=webpack://dotnexus/./Resources/Markup/data-table-vue.html?");

/***/ }),

/***/ "./Resources/Markup/preloader-vue.html":
/*!*********************************************!*\
  !*** ./Resources/Markup/preloader-vue.html ***!
  \*********************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("module.exports = \"<transition name=\\\"fade\\\">\\r\\n    <div class=\\\"preloader-container\\\">\\r\\n        <div class=\\\"preloader-bg\\\"></div>\\r\\n        <div class=\\\"preloader-anim\\\">\\r\\n            <div class=\\\"sk-folding-cube\\\">\\r\\n                <div class=\\\"sk-cube1 sk-cube\\\"></div>\\r\\n                <div class=\\\"sk-cube2 sk-cube\\\"></div>\\r\\n                <div class=\\\"sk-cube4 sk-cube\\\"></div>\\r\\n                <div class=\\\"sk-cube3 sk-cube\\\"></div>\\r\\n            </div>\\r\\n        </div>\\r\\n    </div>\\r\\n</transition>\\r\\n\";\n\n//# sourceURL=webpack://dotnexus/./Resources/Markup/preloader-vue.html?");

/***/ }),

/***/ "./Resources/Scripts/Library/dataTableVue.js":
/*!***************************************************!*\
  !*** ./Resources/Scripts/Library/dataTableVue.js ***!
  \***************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var _Library_preloaderVue__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../Library/preloaderVue */ \"./Resources/Scripts/Library/preloaderVue.js\");\n/* harmony import */ var _Styles_Components_data_table_vue_scss__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../../Styles/Components/_data-table-vue.scss */ \"./Resources/Styles/Components/_data-table-vue.scss\");\n/* harmony import */ var _Styles_Components_data_table_vue_scss__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_Styles_Components_data_table_vue_scss__WEBPACK_IMPORTED_MODULE_2__);\n﻿\r\n\r\n\r\n\r\n\r\n/* harmony default export */ __webpack_exports__[\"default\"] = ({\r\n    template: __webpack_require__(/*! ../../Markup/data-table-vue.html */ \"./Resources/Markup/data-table-vue.html\"),\r\n    props: ['options', 'columns'],\r\n    data: () => {\r\n        return {\r\n            dtOptions: {\r\n                defaultCriteria: {},\r\n                ajaxUrl: '',\r\n                localData: [],\r\n                filters: [],\r\n                showUserFilter: false,\r\n                availableLengths: [10, 25, 50, 100],\r\n                showRowIndex: false,\r\n                paginationLength: 7,\r\n                useQueryString: true,\r\n                customFilterMatch: null\r\n            },\r\n            tableData: {\r\n                pageItems: [],\r\n                totalItems: 0\r\n            },\r\n            defaultCriteria: {\r\n                page: 1,\r\n                length: 10\r\n            },\r\n            criteria: {},\r\n            filter: {\r\n                name: '',\r\n                isUserFilter: false\r\n            },\r\n            isLoading: false,\r\n            baseUrl: `${window.location.protocol}//${window.location.host}/${window.location.pathname.split('/')\r\n                .slice(1).join('/')}`\r\n        };\r\n    },\r\n    computed: {\r\n        dt() {\r\n            return {\r\n                tableData: this.tableData,\r\n                reload: this.dataReload\r\n            };\r\n        },\r\n        dtCriteria() {\r\n            return {\r\n                criteria: this.criteria,\r\n                tableData: this.tableData,\r\n                reload: this.reloadData\r\n            };\r\n        },\r\n        pageCount() {\r\n            return Math.ceil(this.tableData.totalItems / this.criteria.length);\r\n        },\r\n        pageNumbers() {\r\n            const pages = [];\r\n\r\n            if (this.pageCount <= this.dtOptions.paginationLength) {\r\n                for (let i = 0; i < this.pageCount; i++) {\r\n                    pages[i] = i + 1;\r\n                }\r\n            } else {\r\n                const halfway = Math.ceil(this.dtOptions.paginationLength / 2);\r\n\r\n                for (let i = 0; i < this.dtOptions.paginationLength; i++) {\r\n                    if (i === 0) {\r\n                        pages[i] = 1;\r\n                    } else if (i === 1) {\r\n                        pages[i] = this.criteria.page > halfway ? '...' : 2;\r\n                    } else if (i === this.dtOptions.paginationLength - 2) {\r\n                        pages[i] = this.criteria.page < this.pageCount - halfway ? '...' : this.pageCount - 1;\r\n                    } else if (i === this.dtOptions.paginationLength - 1) {\r\n                        pages[i] = this.pageCount;\r\n                    } else {\r\n                        if (this.criteria.page < halfway) {\r\n                            pages[i] = i + 1;\r\n                        } else if (this.criteria.page > this.pageCount - halfway) {\r\n                            pages[i] = this.pageCount - (this.dtOptions.paginationLength - (i + 1));\r\n                        } else {\r\n                            pages[i] = this.criteria.page + (i + 1 - halfway);\r\n                        }\r\n                    }\r\n                }\r\n            }\r\n\r\n            return pages;\r\n        },\r\n        headerRowClass() {\r\n            const ic = this.dtOptions.showIndex ? 'row-index' : '';\r\n            return `row dt-row ${ic}`;\r\n        },\r\n        filterClass() {\r\n            const col = this.options.filterClass ? this.options.filterClass : 'col-12';\r\n            return `custom-filter ${col}`;\r\n        },\r\n        hasLocalData() {\r\n            return this.dtOptions.localData && this.dtOptions.localData.length > 0;\r\n        },\r\n        hasFilters() {\r\n            return this.dtOptions.filters && this.dtOptions.filters.length > 0;\r\n        },\r\n        showUserFilter() {\r\n            return this.dtOptions.showUserFilter || this.filter.isUserFilter;\r\n        }\r\n    },\r\n    watch: {\r\n        criteria() {\r\n            this.reloadData();\r\n        }\r\n    },\r\n    components: {\r\n        Preloader: _Library_preloaderVue__WEBPACK_IMPORTED_MODULE_1__[\"default\"]\r\n    },\r\n    methods: {\r\n        parseRowClass(rowIndex) {\r\n            const r = (rowIndex + 1) % 2 === 0 ? '' : 'odd-row';\r\n            const ic = this.dtOptions.showRowIndex ? 'row-index' : '';\r\n            return `row dt-row ${r} ${ic}`;\r\n        },\r\n        parseColClass(column, classStr, rowIndex) {\r\n            if (!column.class) {\r\n                return 'col';\r\n            }\r\n\r\n            return column.class.includes('col') ? `${classStr} ${column.class}` : `col ${classStr} ${column.class}`;\r\n        },\r\n        pageNumberClass(page) {\r\n            return isNaN(page) || !page ? 'disabled' : page === this.criteria.page ? 'active' : 'enabled';\r\n        },\r\n        getFilterClass(i) {\r\n            return this.dtOptions.filters[i].name === this.filter.name ? 'active-link' : '';\r\n        },\r\n        rowClick(col, rowData) {\r\n            this.$emit('row-click', rowData);\r\n        }, \r\n        changeFilter(filterIndex) {\r\n            if (!this.hasFilters || filterIndex < 0 || this.dtOptions.filters[filterIndex].name === this.filter.name || filterIndex >= this.dtOptions.filters.length) {\r\n                return false;\r\n            }\r\n\r\n            this.filter = this.dtOptions.filters[filterIndex];\r\n\r\n            if (!this.filter.isUserFilter) {\r\n                if (this.dtOptions.showUserFilter) {\r\n                    this.criteria = Object.assign({}, this.defaultCriteria, this.criteria, this.filter.criteria);\r\n                } else {\r\n                    this.criteria = Object.assign({}, this.defaultCriteria, this.filter.criteria);\r\n                }\r\n\r\n                return true;\r\n            }\r\n\r\n            return false;\r\n        },\r\n        changePage(page) {\r\n            if (isNaN(page) || !page || page === this.criteria.page) {\r\n                return;\r\n            }\r\n\r\n            let newPage = this.criteria.page;\r\n\r\n            if (page > this.pageCount) {\r\n                newPage = this.pageCount;\r\n            } else if (page < 1) {\r\n                newPage = 1;\r\n            } else {\r\n                newPage = page;\r\n            }\r\n\r\n            if (newPage === this.criteria.page) {\r\n                return;\r\n            }\r\n\r\n            this.criteria.page = newPage;\r\n            this.reloadData();\r\n        },\r\n        changeLength() {\r\n            this.criteria.page = 1;\r\n            this.reloadData();\r\n        },\r\n        reloadData(resetPage) {\r\n            if (resetPage) {\r\n                this.criteria.page = 1;\r\n            }\r\n\r\n            this.isLoading = true;\r\n\r\n            const criteria = {\r\n                filterCriteria: this.criteria,\r\n                start: (this.criteria.page - 1) * this.criteria.length,\r\n                length: this.criteria.length\r\n            };\r\n\r\n            if (this.dtOptions.ajaxUrl) {\r\n                const self = this;\r\n                jquery__WEBPACK_IMPORTED_MODULE_0___default.a.ajax({\r\n                    url: this.dtOptions.ajaxUrl,\r\n                    type: 'POST',\r\n                    data: criteria,\r\n                    success: (result) => {\r\n                        self.tableData.pageItems = result.data;\r\n                        self.tableData.totalItems = result.recordsFiltered;\r\n                        self.$emit('data-refresh', self.tableData);\r\n                    }\r\n                }).always(() => { self.isLoading = false; });\r\n            } else if (this.hasLocalData) {\r\n                this.tableData.pageItems = this.dtOptions.localData.slice(criteria.start, criteria.start + criteria.length);\r\n                this.tableData.totalItems = this.dtOptions.localData.length;\r\n                this.isLoading = false;\r\n                this.$emit('data-refresh', self.tableData);\r\n            }\r\n\r\n            if (this.dtOptions.useQueryString) {\r\n                this.setUrlQuery();\r\n            }\r\n        },\r\n        setUrlQuery() {\r\n            const params = {};\r\n\r\n            for (let key in this.criteria) {\r\n                if (!this.criteria.hasOwnProperty(key))\r\n                    continue;\r\n\r\n                const prop = this.criteria[key];\r\n                const defProp = this.defaultCriteria[key];\r\n                \r\n                if (prop && prop !== defProp) {\r\n                    params[key] = this.criteria[key];\r\n                }\r\n            }\r\n            \r\n            const p = Object.keys(params).length > 0 ? `?${jquery__WEBPACK_IMPORTED_MODULE_0___default.a.param(params)}` : '';\r\n            const newUrl = `${this.baseUrl}${p}`;\r\n\r\n            if (newUrl !== window.location.href) {\r\n                window.history.pushState('', '', newUrl);\r\n            }\r\n        },\r\n        getUrlQuery() {\r\n            const getUrl = document.location.href;\r\n            const querySplit = getUrl.split('?');\r\n            const query = querySplit.length > 1 ? querySplit[1] : '';\r\n            const result = {};\r\n\r\n            if (query) {\r\n                const pairs = query.split('&');\r\n\r\n                pairs.forEach((pair) => {\r\n                    var kvp = pair.split('=');\r\n\r\n                    var key = kvp[0];\r\n                    var val = decodeURIComponent(kvp[1] || '');\r\n\r\n                    result[key] = key === 'page' || key === 'length' ? parseInt(val) : val;\r\n                });\r\n            }\r\n\r\n            return result;\r\n        },\r\n        matchQueryToFilter(queryObj) {\r\n            let matchedFilterIndex = -1;\r\n\r\n            if (!this.hasFilters) {\r\n                return matchedFilterIndex;\r\n            }\r\n\r\n            if (this.dtOptions.customFilterMatch) {\r\n                return this.dtOptions.customFilterMatch(queryObj);\r\n            }\r\n\r\n            let userIndex = -1;\r\n\r\n            const qo = Object.assign({}, queryObj);\r\n            delete qo.page;\r\n            delete qo.length;\r\n\r\n            this.dtOptions.filters.forEach((filter, i) => {\r\n                if (JSON.stringify(filter.criteria) === JSON.stringify(qo)) {\r\n                    matchedFilterIndex = i;\r\n                } else if (filter.isUserFilter) {\r\n                    userIndex = i;\r\n                }\r\n            });\r\n\r\n            return matchedFilterIndex >= 0 ? matchedFilterIndex : userIndex;\r\n        },\r\n        onPageLoad(isPopState) {\r\n            if (this.dtOptions.useQueryString) {\r\n                const qo = this.getUrlQuery();\r\n\r\n                if (this.hasFilters) {\r\n                    this.changeFilter(this.matchQueryToFilter(qo));\r\n                }\r\n\r\n                const newCriteria = Object.assign({}, this.defaultCriteria, qo);\r\n\r\n                if (!isPopState || JSON.stringify(newCriteria) !== JSON.stringify(this.criteria)) {\r\n                    this.criteria = newCriteria;\r\n                }\r\n            } else {\r\n                this.reloadData();\r\n            }\r\n        }\r\n    },\r\n    mounted() {\r\n        this.dtOptions = Object.assign({}, this.dtOptions, this.options);\r\n        this.defaultCriteria = Object.assign({}, this.defaultCriteria, this.dtOptions.defaultCriteria);\r\n        this.criteria = Object.assign({}, this.criteria, this.defaultCriteria);\r\n        \r\n        window.addEventListener('popstate', () => this.onPageLoad(true));\r\n\r\n        this.onPageLoad();\r\n    }\r\n});\r\n\r\n\n\n//# sourceURL=webpack://dotnexus/./Resources/Scripts/Library/dataTableVue.js?");

/***/ }),

/***/ "./Resources/Scripts/Library/preloaderVue.js":
/*!***************************************************!*\
  !*** ./Resources/Scripts/Library/preloaderVue.js ***!
  \***************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _Styles_Components_preloader_vue_scss__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../Styles/Components/_preloader-vue.scss */ \"./Resources/Styles/Components/_preloader-vue.scss\");\n/* harmony import */ var _Styles_Components_preloader_vue_scss__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_Styles_Components_preloader_vue_scss__WEBPACK_IMPORTED_MODULE_0__);\n﻿\r\n\r\n/* harmony default export */ __webpack_exports__[\"default\"] = ({\r\n    template: __webpack_require__(/*! ../../Markup/preloader-vue.html */ \"./Resources/Markup/preloader-vue.html\"),\r\n    props: [],\r\n    data: () => {\r\n        return {\r\n        };\r\n    }\r\n});\r\n\n\n//# sourceURL=webpack://dotnexus/./Resources/Scripts/Library/preloaderVue.js?");

/***/ }),

/***/ "./Resources/Scripts/blockchain.genesis.js":
/*!*************************************************!*\
  !*** ./Resources/Scripts/blockchain.genesis.js ***!
  \*************************************************/
/*! exports provided: BlockchainGenesis */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"BlockchainGenesis\", function() { return BlockchainGenesis; });\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! vue */ \"./node_modules/vue/dist/vue.js\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var _dicebear_avatars__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @dicebear/avatars */ \"./node_modules/@dicebear/avatars/lib/index.js\");\n/* harmony import */ var _dicebear_avatars__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_dicebear_avatars__WEBPACK_IMPORTED_MODULE_2__);\n/* harmony import */ var _dicebear_avatars_identicon_sprites__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @dicebear/avatars-identicon-sprites */ \"./node_modules/@dicebear/avatars-identicon-sprites/lib/sprites.js\");\n/* harmony import */ var _dicebear_avatars_identicon_sprites__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dicebear_avatars_identicon_sprites__WEBPACK_IMPORTED_MODULE_3__);\n/* harmony import */ var _Library_dataTableVue__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./Library/dataTableVue */ \"./Resources/Scripts/Library/dataTableVue.js\");\n/* harmony import */ var _Styles_blockchain_genesis_scss__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../Styles/blockchain.genesis.scss */ \"./Resources/Styles/blockchain.genesis.scss\");\n/* harmony import */ var _Styles_blockchain_genesis_scss__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_Styles_blockchain_genesis_scss__WEBPACK_IMPORTED_MODULE_5__);\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nclass BlockchainGenesis {\r\n    constructor(options) {\r\n        window.vm = new vue__WEBPACK_IMPORTED_MODULE_1___default.a({\r\n            el: '#body',\r\n            data: {\r\n                identiconSvg: '',\r\n                dtOptions: {\r\n                    localData: options.txs\r\n                }\r\n            },\r\n            components: {\r\n                txTable: _Library_dataTableVue__WEBPACK_IMPORTED_MODULE_4__[\"default\"]\r\n            },\r\n            mounted() {\r\n                if (options.genesis) {\r\n                    const avatars = new _dicebear_avatars__WEBPACK_IMPORTED_MODULE_2___default.a(_dicebear_avatars_identicon_sprites__WEBPACK_IMPORTED_MODULE_3___default.a);\r\n                    const svg = avatars.create(options.genesis);\r\n                    this.identiconSvg = svg;\r\n                }\r\n            }\r\n        });\r\n    }\r\n}\n\n//# sourceURL=webpack://dotnexus/./Resources/Scripts/blockchain.genesis.js?");

/***/ }),

/***/ "./Resources/Styles/Components/_data-table-vue.scss":
/*!**********************************************************!*\
  !*** ./Resources/Styles/Components/_data-table-vue.scss ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("// removed by extract-text-webpack-plugin\n\n//# sourceURL=webpack://dotnexus/./Resources/Styles/Components/_data-table-vue.scss?");

/***/ }),

/***/ "./Resources/Styles/Components/_preloader-vue.scss":
/*!*********************************************************!*\
  !*** ./Resources/Styles/Components/_preloader-vue.scss ***!
  \*********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("// removed by extract-text-webpack-plugin\n\n//# sourceURL=webpack://dotnexus/./Resources/Styles/Components/_preloader-vue.scss?");

/***/ }),

/***/ "./Resources/Styles/blockchain.genesis.scss":
/*!**************************************************!*\
  !*** ./Resources/Styles/blockchain.genesis.scss ***!
  \**************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("// removed by extract-text-webpack-plugin\n\n//# sourceURL=webpack://dotnexus/./Resources/Styles/blockchain.genesis.scss?");

/***/ }),

/***/ 0:
/*!************************!*\
  !*** crypto (ignored) ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("/* (ignored) */\n\n//# sourceURL=webpack://dotnexus/crypto_(ignored)?");

/***/ })

},[["./Resources/Scripts/blockchain.genesis.js","runtime","vendor"]]]);