export default {
    customFetch(input, init){
        return window.fetch(input, {
            ...init,
            credentials: 'same-origin'
        });
    },
};