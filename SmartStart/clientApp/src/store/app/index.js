import { $themeBreakpoints } from '@themeConfig'
import { $themeConfig } from '@themeConfig'
import store from '@/store'
import axiosIns from '@axios'

export default {
    namespaced: true,
    state: {
        percentCompleted: 0,
        isLoading: true,
        domainHost: $themeConfig.app.domainPath,
        windowWidth: 0,
        shallShowOverlay: false,
        prevRoute: null,
        createButtonAndGlobalSearch: {
            isActive: false,
            isbuttonActive: false,
            
            text: '',
            fetchingFunction: null,

            placeHolder: '',
            value: '',
            key: '',
            items: '',
            filterFunc: null
        },
        placeHolderImage: '/media/placeholder/image-placeholder.jpg',
        placeHoldervideo: '/media/placeholder/video-placeholder.jpg',
        cancelTokenSource: axiosIns.CancelToken.source(),
        shownCloseRequest: false
    },
    getters: {
        currentBreakPoint: state => {
            const { windowWidth } = state
            if (windowWidth >= $themeBreakpoints.xl) return 'xl'
            if (windowWidth >= $themeBreakpoints.lg) return 'lg'
            if (windowWidth >= $themeBreakpoints.md) return 'md'
            if (windowWidth >= $themeBreakpoints.sm) return 'sm'
            return 'xs'
        },
        placeHolderImage(state) {
            return state.placeHolderImage;
        },
        placeHoldervideo(state) {
            return state.placeHoldervideo;
        },
        isLoading(state) {
            return state.isLoading;
        },
        percentCompleted(state) {
            return state.percentCompleted;
        },
        cancelTokenSource(state) {
            return state.cancelTokenSource;
        },
        shownCloseRequest(state) {
            return state.shownCloseRequest;
        },
        domainHost(state) {
            return state.domainHost;
        }
    },
    mutations: {
        Update_Prev_Link(state, route) {
            state.prevRoute = route
        },
        UPDATE_WINDOW_WIDTH(state, val) {
            state.windowWidth = val
        },
        TOGGLE_OVERLAY(state, val) {
            state.shallShowOverlay = val !== undefined ? val : !state.shallShowOverlay
        },
        UPDATE_CREATE_BUTTON_AND_GLOBALE_SEARCH(state, newCreateButtonAndGlobalSearch) {
            Object.assign(state.createButtonAndGlobalSearch, newCreateButtonAndGlobalSearch)
        },
        Set_Loading(state, is) {
            state.isLoading = is
            state.percentCompleted = 0
        },
        Percent_Completed(state, payload) {
            state.percentCompleted = payload
        },
        Set_Cancel_Token(state) {
            state.cancelTokenSource = axiosIns.CancelToken.source()
        },
        Shown_Close_Request(state, is) {
            state.shownCloseRequest = is
        }
    },
    actions: {
        pageChanged(context, pageInfo) {
            const firstPageItem = pageInfo.pageLength * (pageInfo.currentPage - 1)
            return pageInfo.items.slice(firstPageItem, firstPageItem + pageInfo.pageLength)
        },
        setLoading({commit}, is) {
            commit('Set_Loading', is)
        },
        filterItems({state}, payload) {
            const {items, key} = payload
            return store.getters[items].filter((item) => {
                return item[key].indexOf(state.createButtonAndGlobalSearch.value) != -1
            })
        },
        setPercentCompleted({commit}, payload) {
            commit("Percent_Completed", payload)
        },
        setCancelToken({commit}) {
            commit("Set_Cancel_Token")
        },
        shownCloseRequest({commit}, is) {
            commit("Shown_Close_Request", is)
        },
    },
}
