import '@mdi/font/css/materialdesignicons.css'
import Vue from 'vue';
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import VueI18n from 'vue-i18n';

Vue.use(Vuetify);
Vue.use(VueI18n);


import tr from "../locale/tr.ts";
import en from "../locale/en.ts";
import de from "../locale/de.ts";
import fr from "../locale/fr.ts";
import es from "../locale/es.ts";
import it from "../locale/it.ts";


let lang = localStorage.getItem("language");



export default new Vuetify({
    icons: {
        iconfont: 'mdi',
    },
    lang: {
        locales: { tr, en, de, fr, es, it },
        current: lang !== '' ? lang : 'en',
    },

});