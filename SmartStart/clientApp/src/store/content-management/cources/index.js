import api from "@api";
import router from "@/router";

export default {
    state: {
        courcesList: [],
        courcesQuestionList: {
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
        courcesDto: {
            id: "",
            name: "",
            year: 0,
            type: 0,
            price: 0,
            isFree: true,
            subjectId: "",
            tagIds: [],
            doctors: [],
            categories: []
        }
    },
    mutations: {
        Cources_List(state, payload) {
            state.courcesList = payload;
        },
        Fetch_Question_Cources(state, payload) {
            state.courcesQuestionList = payload[0];
        },
        Reset_Course_Dto(state, id) {
            if (!id) {
                Object.assign(state.courcesDto, {
                    id: "",
                    name: "",
                    year: 0,
                    type: 0,
                    price: 0,
                    isFree: true,
                    subjectId: "",
                    doctors: [],
                    categories: []
                });
            } else {
                Object.assign(state.courcesDto, {
                    id: state.courcesQuestionList.id,
                    name: state.courcesQuestionList.name,
                    year: state.courcesQuestionList.year,
                    type: state.courcesQuestionList.type,
                    price: state.courcesQuestionList.price,
                    isFree: state.courcesQuestionList.isFree,
                    subjectId: state.courcesQuestionList.subjectId,
                    semesterId: state.courcesQuestionList.semesterId,
                });
            }
        },
        Add_Course(state, payload) {
            state.courcesList.unshift(payload);
        },
        Update_Course(state, payload) {
            state.courcesQuestionList.name = payload.name;
            state.courcesQuestionList.year = payload.year;
            state.courcesQuestionList.type = payload.type;
            state.courcesQuestionList.price = payload.price;
            state.courcesQuestionList.isFree = payload.isFree;
            state.courcesQuestionList.subjectId = payload.subjectId;
            state.courcesQuestionList.semesterId = payload.semesterId;
        },
        delete_Cource_List(state,payload){
            let MapOfIds = new Map(); 
            var idx; 
            var tempList = []; 
            for(idx = 0 ; idx < payload.length ; idx++) {
                 MapOfIds.set(payload[idx] , 1);
            }
            for(idx = 0 ; idx < state.courcesList.length ; idx++) {
                if(MapOfIds.has(state.courcesList[idx].id) === false) {
                    tempList.push(state.courcesList[idx]); 
                }
            }
            state.courcesList = tempList; 
        }
    },
    actions: {
        getCourcesList({ commit }) {
            api.get("Exam/GetAll", ({ data }) => {
                commit("Cources_List", data);
            });
        },
        getQuestionCources({ commit }, id) {
            api.get("Exam/" + id + "/Question/GetAll", ({ data }) => {
                commit("Fetch_Question_Cources", data);
            });
        },
        addCourse({ commit }, payload) {
            api.post("Exam/Add", payload, ({ data }) => {
                commit("Add_Course", data);
            },{
                success:"تم إضافة الدورة بنجاح",
                error:"فشل إضافة الدورة"
            });
        },
        updateCourse({ commit }, payload) {
            api.put("Exam/Update", payload, ({ data }) => {
                commit("Update_Course", data);
            },{
                success:"تم تعديل الدورة بنجاح",
                error:"فشل تعديل الدورة"
            });
        },
        deleteCourse(ctx, id) {
            api.delete("Exam/Delete?id=" + id, ({ data }) => {
                if (data.isSuccess) {
                    router.push("/courses");
                }
            },{
                confirm:"هل تريد فعلا حذف الدورة",
                success:"تم حذف الدورة بنجاح",
                error:"فشل حذف الدورة"
            });
        },
        deleteCourceList({commit},ids){
            api.delete("Exam/MultiDelete",({ data }) => {
                if(data) {
                    commit("delete_Cource_List", ids);
                }
            },{confirm: 'هل تريد فعلا حذف الدورات المحددة', success: 'تم حذف الدورات المحددة بنجاح', error: "فشل حذف الدورات المحددة " },
            ids)
        }
    }
};
