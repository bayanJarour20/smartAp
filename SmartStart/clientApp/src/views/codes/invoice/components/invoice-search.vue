<template>
<activaitor @search="search" placeholder="ابحث عن نقطة بيع محددة"/>
</template>
<script>
import activaitor from "@Ekcore/components/EK-dialog/activaitor";
export default {
    components: {
        activaitor
    },
    methods: {
       search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: [
                    "posName"
                ],
                query   
            })
        },
    }
};
</script>
