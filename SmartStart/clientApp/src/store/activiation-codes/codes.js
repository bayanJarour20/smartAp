import api from "@api";
export default {
    state: {
        codesList: [],
        codeDto: {
            discountRate: 0,
            packageIds: ''
        }
    },
    mutations: {
        get_Codes(state, data) {
            state.codesList = data;
        },
        Generate_Code(state, data) {
            state.codesList.unshift(data)
        }
    },
    actions: {
        getAllCodes({ commit }) {
            api.get("Code/GetAll", ({ data }) => {
                commit("get_Codes", data);
            });
        },
        GenerateCode({commit}, payload) {
            api.post('Code/Generate', payload, ({data}) => {
                commit('Generate_Code', data)
            })
        }
    }
};
