import api from "@api";
import router from "@/router";
export default {
    state: {
        banksList: [],
        banksQuestionList: {
            questions: [],
            dateCreated: "",
            subject: {},
            semesterId: "",
            questionsCount: 0,
            price: 0,
            isFree: false,
            tagIds: [],
            id: "",
            name: "",
            year: 0,
            type: 0,
            subjectId: ""
        },
        bankDto: {
            id: "",
            name: "",
            year: 0,
            type: 0,
            subjectId: "",
            price: 0,
            isFree: false,
            tagIds: []
        }
    },
    mutations: {
        Get_Banks_List(state, payload) {
            state.banksList = payload;
        },
        Fetch_Question_Banks(state, payload) {
            state.banksQuestionList = payload[0];
        },
        Reset_Bank_Dto(state, id) {
            if (!id) {
                Object.assign(state.bankDto, {
                    id: "",
                    name: "",
                    year: 0,
                    type: 0,
                    subjectId: "",
                    price: 0,
                    isFree: false,
                    tagIds: []
                });
            } else {
                Object.assign(state.bankDto, {
                    id: state.banksQuestionList.id,
                    name: state.banksQuestionList.name,
                    year: state.banksQuestionList.year,
                    type: state.banksQuestionList.type,
                    subjectId: state.banksQuestionList.subjectId,
                    price: state.banksQuestionList.price,
                    isFree: state.banksQuestionList.isFree,
                    tagIds: state.banksQuestionList.tagIds
                });
            }
        },
        Add_Bank(state, payload) {
            state.banksList.unshift(payload);
        },
        Update_Bank(state, payload) {
            state.banksQuestionList.name = payload.name;
            state.banksQuestionList.year = payload.year;
            state.banksQuestionList.type = payload.type;
            state.banksQuestionList.price = payload.price;
            state.banksQuestionList.isFree = payload.isFree;
            state.banksQuestionList.subjectId = payload.subjectId;
            state.banksQuestionList.tagIds = payload.tagIds;
        }
    },
    actions: {
        getBanksList({ commit }) {
            api.get("Bank/GetAll", ({ data }) => {
                commit("Get_Banks_List", data);
            });
        },
        getQuestionBanks({ commit }, id) {
            api.get("Bank/" + id + "/Question/GetAll", ({ data }) => {
                commit("Fetch_Question_Banks", data);
            });
        },
        addBank({ commit }, payload) {
            api.post("Bank/Add", payload, ({ data }) => {
                commit("Add_Bank", data);
            });
        },
        updateBank({ commit }, payload) {
            api.put("Bank/Update", payload, ({ data }) => {
                commit("Update_Bank", data);
            });
        },
        deleteBank(ctx, id) {
            api.delete("Bank/Delete/" + id, ({ data }) => {
                if (data.isSuccess) {
                    router.push("/banks");
                }
            });
        }
    }
};
