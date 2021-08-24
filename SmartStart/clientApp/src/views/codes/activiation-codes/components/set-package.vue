<template>
<activaitor @ok="goToAdd" @search="search" placeholder="ابحث عن حزمة محددة" btnText="حزمة جديدة" />
</template>
<script>
import activaitor from "@Ekcore/components/EK-dialog/activaitor";
export default {
    components: {
        activaitor
    },
    methods: {
        goToAdd() {
            this.$router.push('/packages/set/0')
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: ['name', 'description'],
                query   
            })
        },
    }
};
</script>
