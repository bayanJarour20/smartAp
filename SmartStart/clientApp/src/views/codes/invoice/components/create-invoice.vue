<template>
<activaitor @ok="goToAdd" @search="search" placeholder="ابحث عن فاتورة محددة" btnText="فاتورة جديدة" />
</template>
<script>
import activaitor from "@Ekcore/components/EK-dialog/activaitor";
export default {
    components: {
        activaitor
    }, 
    props: {
        id: String
    },
    methods: {
        goToAdd() {
            this.$router.push('/invoice/' + this.id + '/set/0')
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: [
                    "invoiceNumber","codeCount"
                ],
                query   
            })
        },
    }
};
</script>
