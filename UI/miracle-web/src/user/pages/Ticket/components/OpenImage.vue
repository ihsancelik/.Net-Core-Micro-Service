<template>
  <div>
    <v-img :src="image" style="max-height: 1000px; max-width: 1000px"> </v-img>
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
    let that = this;
    document.addEventListener("keyup", function (evt) {
      if (evt.keyCode === 27) {
        that.close();
      }
    });
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
