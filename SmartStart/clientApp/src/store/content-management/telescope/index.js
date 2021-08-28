import api from "@api";
import router from "@/router";
// import store from "@/store";

export default {
    state: {
        telescopeList: [],
        telescopeDto: {
            id: "",
            name: "",
            type: 0,
            subjectId: 0,
            year: 0,
            tagIds: [],
            sections: []
        },
        sectionDto: {
            id: "",
            title: "",
            images: []
        },
        selectedSectionDto: {
            id: "",
            title: "",
            documents: []
        },
        documentIndex: 0,
        typeListDescription: 0
    },
    mutations: {
        Microscope_Get_All(state, payload) {
            state.telescopeList = payload;
        },
        Reset_Telescope_Dto(state) {
            Object.assign(state.telescopeDto, {
                id: "",
                name: "",
                type: 0,
                subjectId: 0,
                year: 0,
                tagIds: [],
            });
        },
        Microscope_Details(state, payload) {
            Object.assign(state.telescopeDto, {
                id: payload.id,
                name: payload.name,
                type: payload.type,
                subjectId: payload.subjectId,
                year: payload.year,
                tagIds: payload.tagIds,
            });
        },
        UPDATE_DOCUMENT_INDEX(state, index) {
            state.documentIndex = index;
        },
        UPDATE_TYPE_LIST_DESCRIPTION(state, type) {
            state.typeListDescription = type;
        },
        delete_Microscope_List(state, payload) {
            let MapOfIds = new Map();
            var idx;
            var tempList = [];
            for (idx = 0; idx < payload.length; idx++) {
                MapOfIds.set(payload[idx], 1);
            }
            for (idx = 0; idx < state.telescopeList.length; idx++) {
                if (MapOfIds.has(state.telescopeList[idx].id) === false) {
                    tempList.push(state.telescopeList[idx]);
                }
            }
            state.telescopeList = tempList;
        }
    },
    actions: {
        microscopeGetAll({ commit }) {
            api.get("Microscope/GetAll", ({ data }) => {
                commit("Microscope_Get_All", data);
            });
        },
        microscopeDetails({ commit }, id) {
            api.get("Microscope/Details/" + id, ({ data }) => {
                commit("Microscope_Details", data);
            });
        },
        addMicroscope(ctx, payload) {
            api.post(
                "Microscope/Add",
                payload,
                ({ data }) => {
                    router.push("/telescope/" + data.id);
                },
                {
                    success: "تم إضافة المجهر بنجاح",
                    error: ".فشلت عملية الإضافة حاول مرة أخرى"
                }
            );
        },
        updateMicroscope(ctx, payload) {
            api.put("Microscope/Update", payload, () => {}, {
                success: "تم تعديل المجهر بنجاح",
                error: ".فشلت عملية التعديل، حاول مرة أخرى"
            });
        },
        deleteMicroscope(ctx, id) {
            api.delete(
                `Microscope/Delete/${id}`,
                ({ data }) => {
                    if (data) {
                        router.push("/telescope/");
                    }
                },
                {
                    confirm: "هل أنت متأكد من حذف هذا المجهر؟",
                    success: "تم حذف المجهر بنجاح.",
                    error: ".فشلت عملية الحذف، حاول مرة أخرى"
                }
            );
        },
        addSectionsMicroscope({ state }, payload) {
            const config = { 
                headers: { 'Content-Type': 'multipart/form-data' } 
            };
            console.log(payload)
            api.post(
                "Microscope/Sections/Add",
                payload,
                ({ data }) => {
                    data.sections.forEach(element => {
                        state.telescopeDto.sections.push(element);
                    });
                },
                {
                    success: "تم إضافة السؤال بنجاح",
                    error: "فشلت عملية الإضافة"
                },
                config
            );
        },
        updateSectionsMicroscope({ state }, payload) {
            const config = {
                headers: { "Content-Type": "multipart/form-data" }
            };
            api.put(
                "Microscope/Sections/Update",
                payload,
                ({ data }) => {
                    Object.assign(state.selectedSectionDto, {
                        id: data.sections[0].id,
                        title: data.sections[0].title,
                        documents: data.sections[0].documents
                    });
                },
                {
                    success: "تم تعديل السؤال بنجاح",
                    error: "فشلت عملية التعديل"
                },
                config
            );
        },
        deleteSectionMicroscope({ state }, id) {
            api.delete(
                `Microscope/Sections/Delete/${id}`,
                ({ data }) => {
                    if (data) {
                        state.telescopeDto.sections.splice(
                            state.telescopeDto.sections.findIndex(
                                sec => sec.id == id
                            ),
                            1
                        );
                        state.selectedSectionDto = [];
                    }
                },
                {
                    confirm: "هل أنت متأكد من حذف هذا السؤال؟",
                    success: "تم حذف السؤال بنجاح.",
                    error: ".فشلت عملية الحذف، حاول مرة أخرى"
                }
            );
        },
        deleteMicroscopeList({ commit }, ids) {
            api.delete(
                "Microscope/deleterange",
                ({ data }) => {
                    if (data) {
                        commit("delete_Microscope_List", ids);
                    }
                },
                {
                    confirm: "هل تريد فعلا حذف المجاهر المحددة",
                    success: "تم حذف المجاهر المحددة بنجاح",
                    error: "فشل حذف المجاهر المحددة "
                },
                ids
            );
        }
    }
};
