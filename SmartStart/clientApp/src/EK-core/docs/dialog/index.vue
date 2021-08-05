<template>
    <div class="p-5 text-right">
        <h1>EK Dialog Width Activitor </h1>
        <EKDialog title="title" placeholder="ابحث " btnText="OPEN">
        </EKDialog>
        <hr>
        <h2>Documentation</h2>
        <h3>dialog</h3>
        <h4># Events</h4>
        <ul class="list-unstyled p-0 m-0">
            <li>1_ open event emit on dialog open</li>
            <li>2_ close event emit on dialog close</li>
            <li>3_ ok event emit on submit button click</li>
            <li>4_ search event emit on @input at search on text input</li>
        </ul>
        <hr>
        <h4># Props</h4>
        <ul class="list-unstyled p-0 m-0">
            <li>1_ placeholder search input place holder  -- type: string</li>
            <li>2_ btnText activator button text -- type: string</li>
            <li>3_ title to show custom title on header -- type: string</li>
            <li>
                4_ endClose to check if you want to close modal after ok event -- type: boolean
            </li>
        </ul>
        <hr>
        <h4># Slots</h4>
        <ul class="list-unstyled p-0 m-0">
            <!-- <li>1_ activator TEST</li> -->
            <li>2_ header</li>
            <li>3_ body</li>
            <li>4_ footer</li>
        </ul>
        <hr>
        <activaitor placeholder="ابحث " btnText="OPEN"/>
        <hr>
        <h3>activaitor</h3>
        <h4># Events</h4>
        <ul class="list-unstyled p-0 m-0">
            <li>1_ search event fired when input at search input</li>
            <li>2_ ok event fired when click on button</li>
        </ul>
        <hr>
        <h4># Props</h4>
        <ul class="list-unstyled p-0 m-0">
            <li>1_ btnText for activitor button -- type: string</li>
            <li>2_ placeholder for search input -- type: string</li>
        </ul>
    </div>
</template>
<style lang="scss" scoped>
ul li, h4 {
    direction: rtl
}
</style>
<script>
import EKDialog from "@Ekcore/components/EK-dialog";
import activaitor from "@Ekcore/components/EK-dialog/activaitor";

export default {
    components: {
        EKDialog,
        activaitor
    }
};
</script>
