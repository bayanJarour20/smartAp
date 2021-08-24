import api from "@api";
import router from "@/router";
export default {
    state: {
        feedbacks: [],
        feedbackDto: {
            id: "",
            title: "",
            body: "",
            appUserId: "",
            appHserName: "",
            reply: "",
            replyDate: null,
            // sendDate: null
        },
        feedbackFilterDto: {
            body: "",

        }
    },
    mutations: {
        Get_Feedbacks_Details(state, payload) {
            state.feedbacks = payload;
        },
        Feedbacks_Details(state, payload) {
            Object.assign(state.feedbackDto, payload)
        },
        Create_Faculty(state, data) {
            state.feedbacks.unshift(data)
        },
        Update_Feedback(state, data) {
            Object.assign(state.feedbacks.find(item => item.id == data.id), data)
        },
        Set_Feedback_Dto(state, payload) {
            Object.assign(
                state.facultyDto,
                payload
            )
        }
    },
    actions: {
        getFeedbackDetails({ commit }) {
            api.get("Feedback/GetAll", ({ data }) => {
                commit("Get_Feedbacks_Details", data);
            });
        },
        getFeedbackDetail({commit}, id) {
            api.get('Feedback/GetById?id=' + id, ({data}) => {
                commit('Feedbacks_Details', data)
            })
        },
        actionFeedback({commit}, payload) {
            api.post('Feedback/Update', payload, ({data}) => {
                if(!payload.id) {
                    commit('Set_Feedback_Dto', data)
                }
            },{success: "تم تعديل الرسالة بنجاح", error: "فشل تعديل الرسالة"})
        },
        deleteFeedback(ctx, id) {
            api.delete("Feedback/Delete?id=" + id, ({ data }) => {
                if(data) {
                    router.push('/contact')
                }
            }, {confirm:"هل أنت متأكد  من حذف هذه الرسالة",success: "تم حذف الرسالة بنجاح", error: "فشل حذف الرسالة"});
        },
    }
};
