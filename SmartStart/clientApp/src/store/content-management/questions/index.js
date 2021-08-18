import api from "@api";
import router from "@/router";
export default {
    state: {
        questonsList: [],
        questionFilterDto: {
            subjectID: 0
        },
        questonsDto: {
            id: "",
            title: "",
            hint: "",
            questionType: 0,
            answerType: 0,
            isCorrected: false,
            images: [],
            // {
            //     sectionImageId
            //     note
            //     path
            //     file
            // }
            dateCreated: new Date(),
            tags: [],
            answers: [],
            // {
            //     id: "",
            //     title: "",
            //     isCorrect: false,
            //     option: "",
            //     correctionDate: new Date(),
            //     questionId: "",
            //     correctAnswerId: ""
            // }
            exams: []
            // {
            //     "examId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            //     "questionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            //     "order": 0,
            //     "examName": "string",
            //     "dateCreated": "2021-06-27T11:11:09.739Z"
            //   }
        }
    },
    getters: {
        filterdCoursesBySubject(state, getters, glState) {
            return glState.cources.courcesList.filter(bank => {
                return (
                    (bank.subjectId == state.questionFilterDto.subjectID ||
                        state.questionFilterDto.subjectID == 0) &&
                    state.questonsDto.exams.findIndex(
                        item => item.examId == bank.id
                    ) == -1
                );
            });
        },
        filterdBanksBySubject(state, getters, glState) {
            return glState.banks.banksList.filter(course => {
                return (
                    (course.subjectId == state.questionFilterDto.subjectID ||
                        state.questionFilterDto.subjectID == 0) &&
                    state.questonsDto.exams.findIndex(
                        item => item.examId == course.id
                    ) == -1
                );
            });
        },
        filterdInterviewsBySubject(state, getters, glState) {
            return glState.interviews.interviewsList.filter(interview => {
                return (
                    (interview.subjectId == state.questionFilterDto.subjectID ||
                        state.questionFilterDto.subjectID == 0) &&
                    state.questonsDto.exams.findIndex(
                        item => item.examId == interview.id
                    ) == -1
                );
            });
        }
    },
    mutations: {
        Get_All_Question(state, payload) {
            state.questonsList = payload;
        },
        Get_Question_Details(state, payload) {
            Object.assign(state.questonsDto, payload);
        },
        Reset_Question_Dto(state) {
            Object.assign(state.questonsDto, {
                id: "",
                title: "",
                hint: "",
                questionType: 0,
                answerType: 0,
                isCorrected: false,
                images: [],
                order: 0,
                dateCreated: new Date(),
                answers: [],
                tags: [],
                exams: []
            });
        },
        Correction(state, payload) {
            state.questonsDto.title = payload.title,
            state.questonsDto.isCorrected = payload.isCorrected
            state.questonsDto.answers = [...payload.answers]
        }
    },
    actions: {
        getAllQuestion({ commit }, payload) {
            api.get(
                "Question/GetAll" +
                    ((payload.examYear || payload.subjectId || payload.semesterId) ? '?' : '') +
                    (payload.examYear ? ("year=" + payload.examYear) : "") +
                    (payload.examYear ? "&" : "") +
                    (payload.subjectId ? ("subjectId=" + payload.subjectId) : "") +
                    ((payload.examYear || payload.subjectId) ? "&" : "") +
                    (payload.semesterId ? ("semesterId=" + payload.semesterId) : ""),
                ({ data }) => {
                    commit("Get_All_Question", data);
                }
            );
        },
        async getQuestionDetails({ commit }, id) {
            return await api.get("Question/Details/" + id, ({ data }) => {
                commit("Get_Question_Details", data);
            });
        },
        setQuestion(ctx, payload) {
            api.post("Question/Add", payload, ({ data }) => {
                console.log(data);
            }, {error: 'error', success: 'success'}, { headers: { 'Content-Type': 'multipart/form-data' } });
        },
        updateQuestion(ctx, payload) {
            api.put("Question/Update", payload, ({ data }) => {
                console.log(data);
            }, {error: 'error', success: 'success'}, { headers: { 'Content-Type': 'multipart/form-data' } });
        },

        correctQuestion({commit}, payload) {
            api.post("Question/Correct", payload, ({ data }) => {
                commit('Correction', data)
            }, {error: 'error', success: 'success'});
        },

        deleteQuestion(ctx, id) {
            api.delete("Question/Delete/" + id, () => {
                router.push('/questions')
            }, {success: "تم حذف السؤال بنجاح", error: "فشل حذف السؤال"});
        },
    }
};
