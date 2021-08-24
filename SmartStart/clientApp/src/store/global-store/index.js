import api from "@api";
import store from "@/store";
export default {
    state: {
        totalTagsList: [],
        universitiesList: [],
        citiesList: [],

        cityDto: {
            id: 0,
            name: ""
        },
        universityDto: {
            id: 0,
            name: "",
            cityId: ""
        },
        tagsDto: {
            id: 0,
            name: "",
            type: 0
        },

        subjectYear: [
            { id: 1, name: "الأولى" },
            { id: 2, name: "الثانية" },
            { id: 3, name: "الثالثة" },
            { id: 4, name: "الرابعة" },
            { id: 5, name: "الخامسة" },
            { id: 6, name: "السادسة" },
            { id: 7, name: "السابعة" }
        ]
    },
    getters: {
        tagsList({ totalTagsList }) {
            console.log(totalTagsList)
            return totalTagsList.filter(item => item.type == 0);
        },
        semester({ totalTagsList }) {
            return totalTagsList.filter(item => item.type == 1);
        },
        doctors({ totalTagsList }) {
            return totalTagsList.filter(item => item.type == 2);
        },
        teams({ totalTagsList }) {
            return totalTagsList.filter(item => item.type == 3);
        },
        sections({ totalTagsList }) {
            return totalTagsList.filter(item => item.type == 4);
        },
        years() {
            let years = [];
            for (let i = 1970; i <= new Date().getFullYear(); i++) {
                years.push({ id: i, name: i });
            }
            return years;
        }
    },
    mutations: {
        Get_Basic_Exams(state, payload) {
            store.state.cources.courcesList = payload.exams;
            store.state.banks.banksList = payload.banks;
        },
        Total_Tag_Fetch(state, payload) {
            console.log(payload)
            state.totalTagsList = payload;
        },
        Fetch_University(state, payload) {
            state.universitiesList = payload;
        },
        Fetch_City(state, payload) {
            state.citiesList = payload;
        },
        
        Create_Tag(state, payload) {
            state.totalTagsList.unshift(payload);
        },
        Create_University(state, payload) {
            state.universitiesList.unshift(payload);
        },
        Create_City(state, payload) {
            state.citiesList.unshift(payload);
        },

        Modify_Tag(state, payload) {
            Object.assign(
                state.totalTagsList.find(item => item.id == payload.id),
                payload
            );
        },
        Modify_University(state, payload) {
            Object.assign(
                state.universitiesList.find(item => item.id == payload.id),
                payload
            );
        },
        Modify_City(state, payload) {
            Object.assign(
                state.citiesList.find(item => item.id == payload.id),
                payload
            );
        },

        Set_Tags_Dto(state, payload) {
            if (payload) {
                Object.assign(state.tagsDto, payload);
            } else {
                Object.assign(state.tagsDto, {
                    id: 0,
                    name: "",
                    type: 0
                });
            }
        },
        Set_University_Dto(state, payload) {
            if (payload) {
                Object.assign(state.universityDto, payload);
            } else {
                Object.assign(state.universityDto, {
                    id: 0,
                    name: "",
                    cityId: ""
                });
            }
        },
        Set_City_Dto(state, payload) {
            if (payload) {
                Object.assign(state.cityDto, payload);
            } else {
                Object.assign(state.cityDto, {
                    id: 0,
                    name: ""
                });
            }
        },

        Delete_City(state, id) {
            state.citiesList.splice(
                state.citiesList.findIndex(item => item.id == id),
                1
            );
        },
        Delete_University(state, id) {
            state.universitiesList.splice(
                state.universitiesList.findIndex(item => item.id == id),
                1
            );
        },
        delete_Tag(state, id) {
            state.totalTagsList.splice(
                state.totalTagsList.findIndex(item => item.id == id),
                1
            );
        },
        delete_City_List(state,payload){
            let MapOfIds = new Map(); 
            var idx; 
            var tempList = []; 
            for(idx = 0 ; idx < payload.length ; idx++) {
                 MapOfIds.set(payload[idx] , 1);
            }
            for(idx = 0 ; idx < state.citiesList.length ; idx++) {
                if(MapOfIds.has(state.citiesList[idx].id) === false) {
                    tempList.push(state.citiesList[idx]); 
                }
            }
            state.citiesList = tempList;
        },
        delete_Tag_List(state,payload){
            let MapOfIds = new Map(); 
            var idx; 
            var tempList = []; 
            for(idx = 0 ; idx < payload.length ; idx++) {
                 MapOfIds.set(payload[idx] , 1);
            }
            for(idx = 0 ; idx < state.totalTagsList.length ; idx++) {
                if(MapOfIds.has(state.totalTagsList[idx].id) === false) {
                    tempList.push(state.totalTagsList[idx]); 
                }
            }
            state.totalTagsList = tempList;
        },
        delete_University_List(state,payload){
            let MapOfIds = new Map(); 
            var idx; 
            var tempList = []; 
            for(idx = 0 ; idx < payload.length ; idx++) {
                 MapOfIds.set(payload[idx] , 1);
            }
            for(idx = 0 ; idx < state.universitiesList.length ; idx++) {
                if(MapOfIds.has(state.universitiesList[idx].id) === false) {
                    tempList.push(state.universitiesList[idx]); 
                }
            }
            state.universitiesList = tempList;
        }
    },
    actions: {
        getBasicExams({ commit }) {
            api.get("Tab/GetBasicExams", ({ data }) => {
                commit("Get_Basic_Exams", data);
            });
        },

        fetchTotalTag({ commit }) {
            api.get("Tag/Fetch", ({ data }) => {
                commit("Total_Tag_Fetch", data);
            });
        },
        fetchUniversity({ commit }) {
            
            api.get("University/GetAll", ({ data }) => {
                commit("Fetch_University", data);
            });
        },
        fetchCity({ commit }) {
            api.get("City/GetAllCites", ({ data }) => {
                commit("Fetch_City", data);
            });
        },
        deleteUniversityList({commit}, ids){
            api.delete("University/DeleteRange",({ data }) => {
                if(data) {
                    commit("delete_University_List", ids);
                }
            },{confirm: 'هل تريد فعلا حذف الجامعات المحددة', success: 'تم حذف الجامعات المحددة بنجاح', error: "فشل حذف الجامعات المحددة " },
            ids)
        },
        createTag({ commit }, payload) {
            if (!payload.id) {
                api.post(
                    "Tag/Add",
                    {
                        name: payload.name,
                        type: payload.type
                    },
                    ({ data }) => {
                        commit("Create_Tag", data);
                    },
                    {
                        success:
                            payload.type == 0
                                ? "تم إضافة الوسم بنجاح"
                                : payload.type == 1
                                ? "تم إضافة الفصل بنجاح"
                                : payload.type == 2
                                ? "تم إضافة الدكتور بنجاح"
                                : "تم إضافة الفريق بنجاح",
                        error:
                            payload.type == 0
                                ? "فشل إضافة الوسم"
                                : payload.type == 1
                                ? "فشل إضافة الفصل"
                                : payload.type == 2
                                ? "فشل إضافة الدكتور"
                                : "فشل إضافة الفريق"
                    }
                );
            } else {
                api.put(
                    "Tag/Modify",
                    {
                        id: payload.id,
                        name: payload.name,
                        type: payload.type
                    },
                    ({ data }) => {
                        commit("Modify_Tag", data);
                    },
                    {
                        success:
                            payload.type == 0
                                ? "تم تعديل الوسم بنجاح"
                                : payload.type == 1
                                ? "تم تعديل الفصل بنجاح"
                                : payload.type == 2
                                ? "تم تعديل الدكتور بنجاح"
                                : "تم تعديل الفريق بنجاح",
                        error:
                            payload.type == 0
                                ? "فشل تعديل الوسم"
                                : payload.type == 1
                                ? "فشل تعديل الفصل"
                                : payload.type == 2
                                ? "فشل تعديل الدكتور"
                                : "فشل تعديل الفريق"
                    }
                );
            }
        },
        createUniversity({ commit }, payload) {
            if (!payload.id) {
                api.post(
                    "University/Add",
                    {
                        name: payload.name,
                        cityId: payload.cityId
                    },
                    ({ data }) => {
                        commit("Create_University", data);
                    },
                    {
                        success: "تم إضافة الجامعة بنجاح",
                        error: "فشل إضافة الجامعة"
                    }
                );
            } else {
                api.put(
                    "University/Update",
                    {
                        id: payload.id,
                        name: payload.name,
                        cityId: payload.cityId
                    },
                    ({ data }) => {
                        commit("Modify_University", data);
                    },
                    {
                        success: "تم تعديل الجامعة بنجاح",
                        error: "فشل تعديل الجامعة"
                    }
                );
            }
        },
        createCity({ commit }, payload) {
            if (!payload.id) {
                api.post(
                    "City/Add",
                    {
                        name: payload.name
                    },
                    ({ data }) => {
                        commit("Create_City", data);
                    },
                    {
                        success: "تم إضافة المدينة بنجاح",
                        error: "فشل إضافة المدينة"
                    }
                );
            } else {
                api.put(
                    "City/Update",
                    {
                        id: payload.id,
                        name: payload.name
                    },
                    ({ data }) => {
                        commit("Modify_City", data);
                    },
                    {
                        success: "تم تعديل المدينة بنجاح",
                        error: "فشل تعديل المدينة"
                    }
                );
            }
        },
        deleteTagList({commit},ids){
            api.delete("Tag/RemoveTags",({ data }) => {
                if(data) {
                    commit("delete_Tag_List", ids);
                }
            },{confirm: 'هل تريد فعلا حذف المدن المحددة', success: 'تم حذف المدن المحددة بنجاح', error: "فشل حذف المدن المحددة " },
            ids)
        },
        deleteCityList({commit}, ids) {
            
            api.delete("City/DeleteRange",({ data }) => {
                if(data) {
                    commit("delete_City_List", ids);
                }
            },{confirm: 'هل تريد فعلا حذف المدن المحددة', success: 'تم حذف المدن المحددة بنجاح', error: "فشل حذف المدن المحددة " },
            ids)
        },
        deleteCity({ commit }, id) {
            api.delete(
                "City/Delete?id=" + id,
                ({ data }) => {
                    if (data) {
                        commit("Delete_City", id);
                    }
                },
                { confirm: "هل انت متأكد من حذف المدينة", success: "تم حذف المدينة بنجاح", error: "فشل حذف المدينة" }
            );
        },
        deleteUniversity({ commit }, id) {
            api.delete("University/Delete?id=" + id,
                ({ data }) => {
                    if (data) {
                        commit("Delete_University", id);
                    }
                },
                {
                    confirm: "هل انت متأكد من حذف الجامعة",
                    success: "تم حذف الجامعة بنجاح",
                    error: "فشل الجامعة المدينة"
                }
            );
        },
        // TODO: complete this
        deleteRangeUniversity(ctx, ids) {
            api.delete("​University/DeleteRange", ids,
                ({ data }) => {
                    if (data) {
                        console.log(data)
                        // commit("Delete_University", id);
                    }
                },
                {
                    confirm: "هل انت متأكد من حذف الجامعات المحددة",
                    success: "تم حذف الجامعات بنجاح",
                    error: "فشل الجامعات المدينة"
                }
            );
        },
        deleteTag({ commit }, payload) {
            api.delete(
                "Tag/Delete?id=" + payload.id,
                ({ data }) => {
                    if (data) {
                        commit("delete_Tag", payload.id);
                    }
                },
                {
                    confirm:
                        payload.type == 0
                            ? "هل انت متأكد من حذف الوسم"
                            : payload.type == 1
                            ? "هل انت متأكد من حذف الفصل"
                            : payload.type == 2
                            ? "هل انت متأكد من حذف الدكتور"
                            : "هل انت متأكد من حذف الفريق",
                    success:
                        payload.type == 0
                            ? "تم حذف الوسم بنجاح"
                            : payload.type == 1
                            ? "تم حذف الفصل بنجاح"
                            : payload.type == 2
                            ? "تم حذف الدكتور بنجاح"
                            : "تم حذف الفريق بنجاح",
                    error:
                        payload.type == 0
                            ? "فشل حذف الوسم"
                            : payload.type == 1
                            ? "فشل حذف الفصل"
                            : payload.type == 2
                            ? "فشل حذف الدكتور"
                            : "فشل حذف الفريق"
                }
            );
        }
    }
};
