import api from "@api";
import router from "@/router";
import store from "@/store";
// import store from "@/store";
export default {
    state: {
        subjectsList: [],
        subjectDto: {
            id: "00000000-0000-0000-0000-000000000000",
            name: "",
            isFree: true,
            file: null,
            type: 0,
            imagePath: "",
            subjectFaculties: [],
            description: "",
            price: 0,
            examCount: 0,
            bankCount: 0,
            microscopeCount: 0,
            interviewCount: 0
        }
    },
    mutations: {
        Fetch_Subject(state, payload) {
            state.subjectsList = payload;
        },
        subject_Create(state, payload) {
            state.subjectsList.unshift(payload);
        },
        Reset_Subject_Dto(state) {
            Object.assign(state.subjectDto, {
                id: "00000000-0000-0000-0000-000000000000",
                name: "",
                isFree: true,
                file: null,
                type: 0,
                imagePath: "",
                subjectFaculties: [],
                description: "",
                price: 0
            });
        },
        Subject_Details(state, payload) {
            payload.subjectFaculties.map((subFuc, index, list) => {
                Object.assign(subFuc, {
                    req: {
                        facultyId: subFuc.facultyId,
                        semesterId: subFuc.semesterId,
                        sectionId: subFuc.sectionId,
                        doctorId: subFuc.doctorId,
                        year: subFuc.year,
                        price: subFuc.price
                    },
                    show: {
                        faculty: subFuc.facultyName,
                        semester: subFuc.semesterName,
                        section: subFuc.sectionName,
                        doctor: subFuc.doctorName,
                        year: store.state.globalStore.subjectYear.find(y => y.id == subFuc.year).name,
                        price: subFuc.price
                    }
                })
                if(list.length - 1 == index) {
                    Object.assign(state.subjectDto, payload);
                }
            })

        },
        subj_List_Dto(state, payload) {
            let MapOfIds = new Map();
            var idx;
            var tempList = [];
            for (idx = 0; idx < payload.length; idx++) {
                MapOfIds.set(payload[idx], 1);
            }
            for (idx = 0; idx < state.subjectsList.length; idx++) {
                if (MapOfIds.has(state.subjectsList[idx].id) === false) {
                    tempList.push(state.subjectsList[idx]);
                }
            }
            state.subjectsList = tempList;
        },
        delete_Subject(state, id) {
            state.subjectsList.splice(
                state.subjectsList.findIndex(item => item.id == id),
                1
            );
        }
    },
    actions: {
        fetchSubject({ commit }, payload) {
            api.get(!payload ? "Subject/GetAll" : "Subject/GetAll?facultyId=" + payload.faculity +
            "&year=" + payload.year + "&semesterId=" + payload.semester, ({ data }) => {
                commit("Fetch_Subject", data);
            });
        },
        setSubject({ commit }, payload) {
            api.post(
                "Subject/SetSubject",
                payload.formData,
                ({ data }) => {
                    if (payload.id == "00000000-0000-0000-0000-000000000000") {
                        commit("subject_Create", data);
                    }
                },
                {
                    success:
                        payload.id == "00000000-0000-0000-0000-000000000000"
                            ? "تم إضافة المادة بنجاح"
                            : "تم تعديل المادة بنجاح",
                    error:
                        !payload.id == "00000000-0000-0000-0000-000000000000"
                            ? "فشل إضافة المادة"
                            : "فشل تعديل المادة"
                }
            );
        },
        subjectDetails({ commit }, id) {
            api.get("Subject/SubjectDetails?subjectId=" + id, ({ data }) => {
                commit("Subject_Details", data);
            });
        },
        deleteSubject({ commit }, id) {
            api.delete(
                "Subject/RemoveSubject?subjectId=" + id,
                ({ data }) => {
                    if (data) {
                        router.push("/subjects");
                        commit("delete_Subject", id);
                    }
                },
                {
                    confirm: "هل تريد فعلاً حذف المادة",
                    success: "تم حذف المادة بنجاح",
                    error: "فشل حذف المادة"
                }
            );
        },
        deleteSubjList({ commit }, ids) {
            api.delete(
                "Subject/RemoveSubjects",
                ({ data }) => {
                    if (data) {
                        commit("subj_List_Dto", ids);
                    }
                },
                {
                    confirm: "هل تريد فعلا حذف المواد المحددة",
                    success: "تم حذف المواد المحددة بنجاح",
                    error: "فشل حذف المواد المحددة "
                },
                ids
            );
        }
    }
};
