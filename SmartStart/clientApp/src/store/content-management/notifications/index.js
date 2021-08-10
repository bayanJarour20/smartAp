import api from "@api";
export default {
    state: {
        notificationList: [],
        notificationDto: {
            id: "",
            title: "",
            body: "",
            date: new Date(),
            notificationType: 0,
            userIds: []
        },
        notificationFilterDto: {
            faculityId: 0,
            cityId: 0
        }
    },
    getters: {
        filterdUserListForNotification(state, getter, glState) {
            return glState.accounts.usersList.filter((user) => {
                console.log(glState.accounts.usersList)
                if(
                    (user.facultyId == state.notificationFilterDto.faculityId || state.notificationFilterDto.faculityId == 0) &&
                    (user.cityId == state.notificationFilterDto.cityId || state.notificationFilterDto.cityId == 0)
                ) {
                    return true
                }
            })
        }
    },
    mutations: {
        Get_Notification(state, payload) {
            state.notificationList = payload;
        },
        Upload_Notification_Create(state, payload) {
            state.notificationList.unshift(payload);
        },
        Reset_Notification_Dto(state) {
            Object.assign(state.notificationDto, {
                id: "",
                title: "",
                body: "",
                date: new Date(),
                notificationType: 0,
                userIds: []
            });
        },
        Delete_Notification(state, payload) {
            state.notificationList.splice(
                state.notificationList.findIndex(item => item.id == payload), 1)
        },
       
        delete_nafitication_List(state,payload){
            let MapOfIds = new Map(); 
            var idx; 
            var tempList = []; 
            for(idx = 0 ; idx < payload.length ; idx++) {
                 MapOfIds.set(payload[idx] , 1);
            }
            for(idx = 0 ; idx < state.notificationList.length ; idx++) {
                if(MapOfIds.has(state.notificationList[idx].id) === false) {
                    tempList.push(state.notificationList[idx]); 
                }
            }
            state.notificationList = tempList; 
         
        }
    },
    actions: {
        newNotification({ commit }, payload) {
            api.post("Notification/Add", payload, ({ data }) => {
                commit("Upload_Notification_Create", data);
            },
            { success: 'تم إضافة الإشعار بنجاح', error: "فشل إضافة الإشعار المحددة " });
        },
        getNotification({ commit }) {
            api.get("Notification/GetAll", ({ data }) => {
                commit("Get_Notification", data);
            });
        },
        deletenafiticationList({commit},ids){
            api.delete("Notification/DeleteRange",({ data }) => {
                if(data) {
                    commit("delete_nafitication_List", ids);
                }
            },{confirm: 'هل تريد فعلا حذف الإشعارات المحددة', success: 'تم حذف الإشعارات المحددة بنجاح', error: "فشل حذف الإشعارات المحددة " },
            ids)
        },
        deleteNotification({ commit }, payload) {
            api.delete("Notification/Delete?id=" + payload, ({ data }) => {
                if(data.isSuccess) {
                    commit("Delete_Notification", payload);
                }
            },{confirm: 'هل تريد فعلا حذف الإشعار ', success: 'تم حذف الإشعار  بنجاح', error: "فشل حذف الإشعار  " });
        }
    }
};
