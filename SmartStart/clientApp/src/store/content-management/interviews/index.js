import api from "@api";
import router from "@/router";
export default {
    state: {
        interviewsList: [],
        interviewQuestionList: {
            questions: [],
            dateCreated: "",
            subject: {},
            semesterId: "",
            questionsCount: 0,
            tagIds: [],
            id: "",
            name: "",
            year: 0,
            type: 0,
            subjectId: ""
        },
        interviewsDto: {
            id: "",
            name: "",
            year: 0,
            type: 0,
            subjectId: "",
            tagIds: []
        }
    },
    mutations: {
        Fetch_Interviews(state, payload) {
            state.interviewsList = payload;
        },
        Fetch_Interviews_Details(state, payload) {
            state.interviewQuestionList = payload;
        },
        Reset_Interviews_Dto(state, id) {
            if (!id) {
                Object.assign(state.interviewsDto, {
                    id: "",
                    name: "",
                    year: 0,
                    type: 0,
                    subjectId: "",
                    tagIds: []
                });
            } else {
                Object.assign(state.interviewsDto, {
                    id: state.interviewQuestionList.id,
                    name: state.interviewQuestionList.name,
                    year: state.interviewQuestionList.year,
                    type: state.interviewQuestionList.type,
                    subjectId: state.interviewQuestionList.subjectId,
                    tagIds: state.interviewQuestionList.tagIds
                });
            }
        },
        Add_Interviews(state, payload) {
            state.interviewsList.unshift(payload);
        },
        Update_Interview(state, payload) {
            state.interviewQuestionList.name = payload.name;
            state.interviewQuestionList.year = payload.year;
            state.interviewQuestionList.type = payload.type;
            state.interviewQuestionList.subjectId = payload.subjectId;
            state.interviewQuestionList.tagIds = payload.tagIds;
        },
        delete_Interview_List(state, payload) {
            let MapOfIds = new Map();
            var idx;
            var tempList = [];
            for (idx = 0; idx < payload.length; idx++) {
                MapOfIds.set(payload[idx], 1);
            }
            for (idx = 0; idx < state.interviewsList.length; idx++) {
                if (MapOfIds.has(state.interviewsList[idx].id) === false) {
                    tempList.push(state.interviewsList[idx]);
                }
            }
            state.interviewsList = tempList;
        }
    },
    actions: {
        fetchInterviews({ commit }) {
            api.get("Interview/GetAll", ({ data }) => {
                commit("Fetch_Interviews", data);
            });
        },
        fetchInterviewsDetails({ commit }, id) {
            api.get("Interview/" + id + "/Question/GetAll", ({ data }) => {
                commit("Fetch_Interviews_Details", data[0]);
            });
        },
        addInterview({ commit }, payload) {
            api.post("Interview/Add", payload, ({ data }) => {
                commit("Add_Interviews", data);
            },
            {
                success: "تم إضافة السؤال الكتابي بنجاح",
                error: "فشل إضافة السؤال الكتابي"
            });
        },
        updateInterview({ commit }, payload) {
            api.put("Interview/Update", payload, ({ data }) => {
                commit("Update_Interview", data);
            },
            {
                success: "تم تعديل السؤال الكتابي بنجاح",
                error: "فشل تعديل السؤال الكتابي"
            });
        },
        deleteInterview(ctx, id) {
            api.delete("Interview/Delete/" + id, ({ data }) => {
                if (data.isSuccess) {
                    router.push("/interviews");
                }
            },
            {
                confirm: "هل تريد فعلا حذف السؤال الكتابي",
                success: "تم حذف السؤال الكتابي بنجاح",
                error: "فشل حذف السؤال الكتابي"
            });
        },
        deleteInterviewList({ commit }, ids) {
            api.delete(
                "Interview/deleterange",
                ({ data }) => {
                    if (data) {
                        commit("delete_Interview_List", ids);
                    }
                },
                {
                    confirm: "هل تريد فعلا حذف الاسئلة الكتابية المحددة",
                    success: "تم حذف الاسئلة الكتابية المحددة بنجاح",
                    error: "فشل حذف الاسئلة الكتابية المحددة "
                },
                ids
            );
        }
    }
};
