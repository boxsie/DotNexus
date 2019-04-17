const path = require('path');
const webpack = require('webpack');
const extractTextPlugin = require('extract-text-webpack-plugin');
const AssetsPlugin = require('assets-webpack-plugin');

const webRoot = path.resolve(__dirname, 'wwwroot');
const baseScriptsPath = './Resources/Scripts/';
const chunkFileNameJs = 'js/[name].js';
const chunkFileNameCss = 'css/[name].css';

module.exports = {
    mode: 'development',
    entry: {
        'main': baseScriptsPath + 'main.js',
        'blockchain.blocks': baseScriptsPath + 'blockchain.blocks.js'
    },
    output: {
        path: webRoot,
        publicPath: '../',
        chunkFilename: chunkFileNameJs,
        filename: chunkFileNameJs,
        library: 'dotnexus'
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                vendor: {
                    chunks: 'all',
                    name: 'vendor',
                    test: /node_modules/,
                    enforce: true,
                    priority: 20
                }
            }
        },
        runtimeChunk: 'single'
    },
    module: {
        rules: [
            {
                test: /\.css$/,
                use: extractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: ['css-loader']
                })
            },
            {
                test: /\.scss$/,
                use: extractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: ['css-loader', 'sass-loader']
                })
            },
            {
                test: /\.html$/,
                use: 'html-loader'
            },
            {
                test: /\.sass$/,
                loaders: ['raw', 'sass']
            },
            {
                test: /\.woff2?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                use: 'url-loader'
            },
            {
                test: /\.(jpg|jpeg|gif|png)$/,
                use: 'url-loader?limit=1024&name=img/[name].[ext]'
            },
            {
                test: /\.(woff|woff2|eot|ttf|svg)$/,
                use: 'url-loader?limit=1024&name=font/[name].[ext]'
            },
            {
                test: /\.vue$/,
                use: 'vue-loader'
            },
            {
                test: /bootstrap\/dist\/js\/umd\//,
                use: 'imports-loader?jQuery=jquery'
            }
        ]
    },
    resolve: {
        alias: {
            'vue': 'vue/dist/vue.js'
        }
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: 'jquery',
            "window.jQuery": 'jquery',
            Tether: 'tether',
            'window.Tether': 'tether',
            Alert: 'exports-loader?Alert!bootstrap/js/dist/alert',
            Button: 'exports-loader?Button!bootstrap/js/dist/button',
            Carousel: 'exports-loader?Carousel!bootstrap/js/dist/carousel',
            Collapse: 'exports-loader?Collapse!bootstrap/js/dist/collapse',
            Dropdown: 'exports-loader?Dropdown!bootstrap/js/dist/dropdown',
            Modal: 'exports-loader?Modal!bootstrap/js/dist/modal',
            Popover: 'exports-loader?Popover!bootstrap/js/dist/popover',
            Scrollspy: 'exports-loader?Scrollspy!bootstrap/js/dist/scrollspy',
            Tab: 'exports-loader?Tab!bootstrap/js/dist/tab',
            Tooltip: 'exports-loader?Tooltip!bootstrap/js/dist/tooltip',
            Util: 'exports-loader?Util!bootstrap/js/dist/util',
            Vue: 'vue'
        }),
        new extractTextPlugin({
            filename: chunkFileNameCss
        })
        //new AssetsPlugin({
        //    filename: 'App_Data/webpack.assets.json',
        //    path: __dirname,
        //    prettyPrint: true
        //})
    ]
};