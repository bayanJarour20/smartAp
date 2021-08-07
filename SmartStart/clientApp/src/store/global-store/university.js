import api from "@api";
export default {
    state: {
        universities: []
    },
    getters: {},
    mutations: {
        Fetch_University(state, payload) {
            state.universities = payload
        }
    },
    actions: {
        fetchUniversity({commit}) {
            api.get("University/Fetch", ({ data }) => {
                commit('Fetch_University', data)
            })
        }
    }
}