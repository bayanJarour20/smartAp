<template>
    <div>       <!-- // selectedLabel --- opthinal props - default value: id -->
        <EKTable
            :items="facultiesList"
            :columns="columns"
            selectedLabel="id"
            @details="openEditFaculityDialog"
            @delete-selected="fireDeleteEvent"
        >
        </EKTable>
        <createFacultie ref="editFacultieDialog" title="تعديل كلية" isEdit />
        
    </div>
</template>
<script>
import createFacultie from "./components/create-facultie";
import EKTable from "@Ekcore/components/EK-table";
import { mapActions, mapGetters } from 'vuex';
export default {
    components: {
        EKTable,
        createFacultie
    },
    computed: {
        ...mapGetters(['facultiesList'])
    },
    data: () => ({
        columns: [
            {
                label: "اسم الكلية",
                field: "name"
            },
            {
                label: "عدد السنوات",
                field: "numOfYears",
               
            },
            {
                label: "تفاصيل",
                field: "details",
                sortable: false
            }
        ]
    }),
    created() {
        this.getFacultiesDetails()
    },
    methods: {
        ...mapActions(["getFacultiesDetails","deleteFacultyList"]),
        openEditFaculityDialog(p) {
            this.$store.commit('Set_Facultie_Dto', p.row)
            this.$refs.editFacultieDialog.openDialog();
        },
        fireDeleteEvent(list) {
           
          this.deleteFacultyList(list)
        }
    },
    beforeDestroy() {
        this.$store.commit('Reset_Search_Dto')
    }
};
</script>
