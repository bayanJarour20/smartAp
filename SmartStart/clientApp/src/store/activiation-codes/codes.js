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
        },
        Code_List_Dto(state,payload){
            let MapOfIds = new Map(); 
            var idx; 
            var tempList = []; 
            for(idx = 0 ; idx < payload.length ; idx++) {
                 MapOfIds.set(payload[idx] , 1);
            }
            for(idx = 0 ; idx < state.codesList.length ; idx++) {
                if(MapOfIds.has(state.codesList[idx].id) === false) {
                    tempList.push(state.codesList[idx]); 
                }
            }
            state.codesList = tempList; 
         
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
        },
        CodeListDto({commit},ids){
            api.delete("Code/RemoveCodes",({ data }) => {
                if(data) {
                    commit("Code_List_Dto", ids);
                }
            },{confirm: 'هل تريد فعلا حذف الأكواد المحددة', success: 'تم حذف الأكواد المحددة بنجاح', error: "فشل حذف الأكواد المحددة " },
            ids)
        }
    }
};
