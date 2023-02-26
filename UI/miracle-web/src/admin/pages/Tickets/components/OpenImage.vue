<template>
  <div>
    <v-img :src="image" style="max-height: 1000px; max-width: 1000px;">
      <a @click="close(false)"> <v-icon style="float: right;">mdi-close-circle </v-icon> </a>
    </v-img>
  </div>
</template>

<script>
import { Base } from "@/helpers/RouteConstraints";
import TicketService from "@/services/TicketService";

export default {
  props: ["messageIdProp"],
  data() {
    return {
      image: "",

      signalModel: {
        changes: false,
        returnValues: null,
      },
    };
  },
  created() {
    this.initialize();
  },
  methods: {
    async initialize() {
      let imageResponse = await new TicketService().getImage(this.messageIdProp);
      this.image = Base + imageResponse;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
};
</script>
