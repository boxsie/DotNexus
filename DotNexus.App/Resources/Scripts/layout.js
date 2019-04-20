import $ from 'jquery';
import Vue from 'vue';
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import Avatars from '@dicebear/avatars';
import SpriteCollection from '@dicebear/avatars-identicon-sprites';

export default {
    install(Vue, options) {
        Vue.prototype.$layout = new Vue({
            el: '#layout',
            data: {
                latestBlock: {},
                identiconSvg: ''
            },
            methods: {
                parseBlockChannel(channel) {
                    return channel;
                },
                parseTxType(txType) {
                    return txType;
                },
                parseBytes(bytes) {
                    const sizes = ['B', 'KB', 'MB', 'GB', 'TB'];
                    if (bytes === 0) return '0 Byte';
                    const i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
                    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
                }
            },
            mounted() {
                $('.toast').toast({ delay: 5000 });

                this.connection = new HubConnectionBuilder()
                    .configureLogging(LogLevel.Information)
                    .withUrl('/blockchainhub')
                    .build();

                this.connection.start()
                    .then(() => {
                        this.connection.on('newBlockNotify', (block) => {
                            this.latestBlock = block;
                            this.$emit('new-block-notify', this.latestBlock);
                            $('#newBlockToast').toast('show');
                        });
                    });

                if (options.userGenesis) {
                    const avatars = new Avatars(SpriteCollection);
                    const svg = avatars.create(options.userGenesis);
                    this.identiconSvg = svg;
                }
            }
        });
    }
}