<template>
    <div class="home">
        <h1>{{ msg }}</h1>
        <div class="input-group mb-3">
            <b-input-group class="mt-2"  label-cols-sm="1">
                <b-form-input v-model="searchText" placeholder="Search"></b-form-input>
                <b-input-group-append>
                    <b-button variant="info" v-on:click="searchBooks">Search</b-button>
                    <b-button :to="{ name: 'add_book_view' }" variant="secondary">Add Book</b-button>
                </b-input-group-append>
            </b-input-group>
        </div>
        <b-table id="my-table" striped hover :items="items" :fields="fields" responsive="sm">
            <template v-slot:cell(thumbnailUrl)="data">
                <b-img :src="data.value" thumbnail fluid></b-img>
            </template>
            <template v-slot:cell(title_link)="data">
                <b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
            </template>
        </b-table>
        <b-pagination v-model="currentPage"
                      :total-rows="totalRecords"
                      :per-page="perPage"
                      aria-controls="my-table">
        </b-pagination>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'Home',
        props: {
            msg: String
        },
        data: () => (
            {
            searchText:'',
            perPage: 10,
            currentPage: 1,
            totalRecords: 0,
            fields: [
                { key: 'thumbnailUrl', label: 'Book Image' },
                { key: 'title_link', label: 'Book Title', sortable: true, sortDirection: 'desc' },
                { key: 'isbn', label: 'ISBN', sortable: true, sortDirection: 'desc' },
                { key: 'descr', label: 'Description', sortable: true, sortDirection: 'desc' }

            ],
            items: []
            }
        ),
        created() {
            this.getAllBooks();
        },
        
        watch: { 
            currentPage: function (newVal, oldVal) {
                /* eslint-disable no-console */
                console.log('Prop changed: ', newVal, ' | was: ', oldVal)
                /* eslint-enable no-console */
                this.getAllBooks();
            }
        },
        methods: {
            searchBooks() {
                this.currentPage = 1;
                this.getAllBooks();
            },
            getAllBooks() {                
                axios.get("https://localhost:5001/books",
                    {
                        params: {
                            searchText: this.searchText,
                            pageNumber: this.currentPage,
                            batchSize: this.perPage
                        }
                    }
                    )
                    .then(response => {
                        this.items = response.data.items;
                        this.totalRecords = response.data.totalRecords;
                    });
            },
            dataContext(ctx, callback) {
                axios.get("https://localhost:5001/books/")
                    .then(response => {
                        
                        callback(response.data);
                    });
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

