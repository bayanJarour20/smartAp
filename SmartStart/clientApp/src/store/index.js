import Vue from 'vue'
import Vuex from 'vuex'

// Modules
import app from './app'
import appConfig from './app-config'
import verticalMenu from './vertical-menu'

import filter from './global-store/filter'
import globalStore from './global-store'

import codes from "./activiation-codes/codes.js"
import packages from "./activiation-codes/packages.js"
import faculties from "./content-management/faculties"
import advertising from './content-management/advertising'
import invoices from "./invoices"
import cources from './content-management/cources'
import questions from './content-management/questions'
import banks from './content-management/banks'



import subjects from './content-management/subjects'

import natification from "./content-management/notifications"

import accounts from './accounts'
import feedbacks from "./content-management/contact"
import home from "./home" 


Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    app,
    cources,
    appConfig,
    verticalMenu,
    codes,
    packages,
    accounts,
    invoices,
    home,
    questions,
    banks,
    advertising,

    faculties,
    feedbacks,
    filter,
    globalStore,
    subjects,
   
    natification
  },
  strict: process.env.DEV,
})
