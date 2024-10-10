document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('form');
    const productIdInput = document.getElementById('productId');
    const productNameInput = document.getElementById('productName');
    const productPriceInput = document.getElementById('productPrice');
    const productsTable = document.getElementById('products');

    const apiUrl = 'https://localhost:7194/api/Product'; 

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const product = {
            name: productNameInput.value,
            price: parseFloat(productPriceInput.value)
        };

        if (productIdInput.value) {

            await fetch(${apiUrl}/${productIdInput.value}, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(product)
            });
        } else {
            await fetch(apiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(product)
            });
        }

        form.reset();
        productIdInput.value = '';
        loadProducts();
    });

    productsTable.addEventListener('click', async (e) => {
        if (e.target.classList.contains('edit')) {
            const id = e.target.dataset.id;
            const response = await fetch(${apiUrl}/${id});
            const product = await response.json();

            productIdInput.value = product.id;
            productNameInput.value = product.name;
            productPriceInput.value = product.price;
        }

        if (e.target.classList.contains('delete')) {
            const id = e.target.dataset.id;
            await fetch(${apiUrl}/${id}, {
                method: 'DELETE'
            });
            loadProducts();
        }
    });

    async function loadProducts() {
        const response = await fetch(apiUrl);
        const products = await response.json();

        productsTable.innerHTML = products.map(product => `
            <tr>
                <td data-label="Name">${product.name}</td>
                <td data-label="Price">${product.price}</td>
                <td data-label="Actions">
                    <button class="edit" data-id="${product.id}">Edit</button>
                    <button class="delete" data-id="${product.id}">Delete</button>
                </td>
            </tr>
        `).join('');
    }

    loadProducts();
});