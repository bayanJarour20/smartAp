<template>
    <b-container>
        <b-row>
            <b-col cols="12" md="6" lg="4">
                <EKTable
                    title="المدن"
                    :items="citiesList"
                    :columns="citiesColumn"
                    isPlus
                    selectedLabel="id"
                    @plus="setCityDialogForm()"
                    @details="setCityDialogForm($event)"
                    @delete-selected="deleteCity"
                >
                </EKTable>
            </b-col>
            <b-col cols="12" md="6" lg="4">
                <EKTable
                    title="الجامعات"
                    :items="universitiesList"
                    :columns="universitiesColumn"
                    isPlus
                    selectedLabel="id"
                    @plus="setUniversityDialogForm()"
                    @details="setUniversityDialogForm($event)"
                    @delete-selected="deleteUniversity"
                >
                </EKTable>
            </b-col>
            <b-col cols="12" md="6" lg="4">
                <EKTable
                    title="الوسوم"
                    :items="tagsList"
                    :columns="tagsColumn"
                    isPlus
                    @plus="setTagDialogForm(0)"
                    @details="setTagDialogForm(0, $event)"
                    @delete-selected="deleteTag"
                >
                </EKTable>
            </b-col>
            <b-col cols="12" md="6" lg="4">
              
                <EKTable
                    title="الفصول"
                    :items="semester"
                    :columns="semesterColumn"
                    isPlus
                    @plus="setTagDialogForm(1)"
                    @details="setTagDialogForm(1, $event)"
                    @delete-selected="deleteTag"
                >
                </EKTable>
            </b-col>
             <b-col cols="12" md="6" lg="4">
                <EKTable
                    title="الأقسام"
                    :items="sections"
                    :columns="sectionColumn"
                    isPlus
                    @plus="setTagDialogForm(4)"
                    @details="setTagDialogForm(4, $event)"
                    @delete-selected="deleteTag"
                >
                </EKTable>
            </b-col>
            <b-col cols="12" md="6" lg="4">
                <EKTable
                    title="الدكاترة"
                    :items="doctors"
                    :columns="doctorsColumn"
                    isPlus
                    @plus="setTagDialogForm(2)"
                    @details="setTagDialogForm(2, $event)"
                    @delete-selected="deleteTag"
                >
                </EKTable>
            </b-col>
            <b-col cols="12" md="6" lg="4">
                <EKTable
                    title="الفرق"
                    :items="teams"
                    :columns="teamsColumn"
                    isPlus
                    @plus="setTagDialogForm(3)"
                    @details="setTagDialogForm(3, $event)"
                    @delete-selected="deleteTag"
                >
                </EKTable>
            </b-col>
        </b-row>
        <city ref="cityDialog" />
        <university ref="universityDialog" />
        <tag ref="tagDialog" />
        <semester ref="semseterDialog" />
        <sections ref="sectionsDialog" />
        <doctor ref="doctorDialog" />
        <team ref="teamDialog" />
</b-container>
</template>
<script>
import EKTable from "@Ekcore/components/EK-table";
import { mapState, mapGetters, mapActions } from "vuex";

import city from "@/views/settings/components/city";
import university from "@/views/settings/components/university";
import tag from "@/views/settings/components/tag";
import semester from "@/views/settings/components/semester";
import doctor from "@/views/settings/components/doctor";
import team from "@/views/settings/components/team";
import sections from "@/views/settings/components/sections";
export default {
    components: {
        EKTable,

        city,
        university,
        tag,
        semester,
        sections,
        doctor,
        team
    },
    data: () => ({
        dialogsInfo: [
            {
                name: "city"
            },
            {
                name: "university"
            },
            {
                name: "tag"
            },
            {
                name: "semester"
            },
            {
                name: "doctor"
            },
            {
                name: "team"
            },
            {
                name: "sections"
            }
        ],
        citiesColumn: [
            {
                label: "اسم المدينة",
                field: "name"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ],
        universitiesColumn: [
            {
                label: "اسم الجامعة",
                field: "name"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ],
        tagsColumn: [
            {
                label: "اسم الوسم",
                field: "name"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ],
        semesterColumn: [
            {
                label: "اسم الفصل",
                field: "name"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ],
        sectionColumn: [
            {
                label: "اسم القسم",
                field: "name"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ],
        doctorsColumn: [
            {
                label: "اسم الدكتور",
                field: "name"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ],
        teamsColumn: [
            {
                label: "اسم الفريق",
                field: "name"
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ]
    }),
    computed: {
        ...mapState({
            citiesList: state => state.globalStore.citiesList,
            universitiesList: state => state.globalStore.universitiesList,
        }),
        ...mapGetters(["tagsList", "semester", "doctors", "teams", "sections"])
    },

    created() {
        // this.fetchTotalTag();
        this.fetchUniversity();
        this.fetchCity();
        this.fetchTotalTag();
    },

    methods: {
        ...mapActions([
            "fetchTotalTag",
            "fetchUniversity",
            "fetchCity",
            "deleteCityList",
            "deleteUniversityList",
            "deleteTagList"
        ]),
        setCityDialogForm(item) {
            if (!item) {
                this.$store.commit("Set_City_Dto");
            } else {
                this.$store.commit("Set_City_Dto", item.row);
            }
            this.$refs.cityDialog.open()
        },
        setUniversityDialogForm(item) {
            if (!item) {
                this.$store.commit("Set_University_Dto");
            } else {
                this.$store.commit("Set_University_Dto", item.row);
            }
            this.$refs.universityDialog.open()
        },
        setTagDialogForm(type, item) {
            if (!item) {
                this.$store.commit("Set_Tags_Dto");
            } else {
                this.$store.commit("Set_Tags_Dto", item.row);
            } 
            if (type == 0) {
                this.$refs.tagDialog.open()
            } else if (type == 1) {
                this.$refs.semseterDialog.open()
            } else if (type == 2) {
                this.$refs.doctorDialog.open()
            } else if (type == 3) {
                this.$refs.teamDialog.open()
            } else if (type == 4) {
                this.$refs.sectionsDialog.open()
            }
        },
        deleteCity(list){
            this.deleteCityList(list)
        },
        deleteTag(list){
            this.deleteTagList(list)
        },
        deleteUniversity(list){
          this.deleteUniversityList(list)
        }
    }
};
</script>
