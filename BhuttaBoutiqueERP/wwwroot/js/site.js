/* ================================================================= */
/* Bhutta Boutique ERP - Global Client-Side Application Logic (Fixed) */
/* ================================================================= */

const moduleFeatures = {
    "Inventory": [
        { name: "Item Master", url: "/Inventory/Items", icon: "fa-cube" },
        { name: "Stock In (GRN)", url: "/Inventory/StockIn", icon: "fa-boxes-stacked" },
        { name: "Stock Out", url: "/Inventory/StockOut", icon: "fa-truck-fast" },
        { name: "Stock Return", url: "/Inventory/StockReturn", icon: "fa-rotate-left" },
        { name: "Suppliers", url: "/Inventory/Suppliers", icon: "fa-truck-field" },
        { name: "Wastage / Scrap", url: "/Inventory/Wastage", icon: "fa-trash-can" },
        { name: "Low Stock Alert", url: "/Inventory/LowStock", icon: "fa-triangle-exclamation" },
        { name: "Stock Reports", url: "/Inventory/Reports", icon: "fa-chart-column" }
    ],
    "Services": [
        { name: "Service Catalog", url: "/Services/Catalog", icon: "fa-book-open" },
        { name: "Customer Management", url: "/Services/Customers", icon: "fa-address-book" },
        { name: "Order Booking", url: "/Services/Orders", icon: "fa-cart-shopping" },
        { name: "Measurements", url: "/Services/Measurements", icon: "fa-ruler-combined" },
        { name: "Design Library", url: "/Services/DesignLibrary", icon: "fa-palette" },
        { name: "Payment Management", url: "/Services/Payments", icon: "fa-file-invoice-dollar" },
        { name: "Service Reports", url: "/Services/Reports", icon: "fa-chart-pie" }
    ],
    "Production": [
        { name: "Fabric Demand", url: "/Production/FabricDemand", icon: "fa-clipboard-list" },
        { name: "Demand List", url: "/Production/DemandList", icon: "fa-list-check" },
        { name: "Stock Check", url: "/Production/StockCheck", icon: "fa-warehouse" },
        { name: "Fabric Allocation", url: "/Production/FabricAllocation", icon: "fa-arrows-split-up-and-left" },
        { name: "Fabric Issue (DO)", url: "/Production/FabricIssue", icon: "fa-file-export" },
        { name: "Order Lifecycle", url: "/Production/OrderLifecycle", icon: "fa-timeline" },
        { name: "Production Reports", url: "/Production/ProductionReports", icon: "fa-chart-line" }
    ],
    "HR": [
        { name: "Employee Master", url: "/HR/EmployeeMaster", icon: "fa-id-badge" },
        { name: "Compensation Matrix", url: "/HR/Compensation", icon: "fa-money-bill-wave" },
        { name: "Attendance", url: "/HR/Attendance", icon: "fa-calendar-check" },
        { name: "Leave Management", url: "/HR/Leave", icon: "fa-calendar-minus" },
        { name: "Advance / Loan", url: "/HR/AdvanceLoan", icon: "fa-hand-holding-dollar" },
        { name: "Payroll Processing", url: "/HR/Payroll", icon: "fa-file-invoice" },
        { name: "HR Reports", url: "/HR/Reports", icon: "fa-chart-area" }
    ],
    "Reports": [
        { name: "Dashboard", url: "/Reports/Dashboard", icon: "fa-gauge" },
        { name: "Sales Reports", url: "/Reports/Sales", icon: "fa-sack-dollar" },
        { name: "Inventory Reports", url: "/Reports/Inventory", icon: "fa-boxes-stacked" },
        { name: "Production Reports", url: "/Reports/Production", icon: "fa-industry" },
        { name: "HR Reports", url: "/Reports/HR", icon: "fa-users-line" },
        { name: "Financial Reports", url: "/Reports/Financial", icon: "fa-file-circle-check" },
        { name: "Advanced Features", url: "/Reports/Advanced", icon: "fa-wand-magic-sparkles" }
    ]
};

$(document).ready(function () {
    // Ensure Toast Container exists in DOM
    if ($('#toastContainer').length === 0) {
        $('body').append('<div id="toastContainer" class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 1090; margin-top: 65px;"></div>');
    }

    // 🌟 SMART AUTO-HIGHLIGHT: URL match karke sahi module aur sub-menu highlight karega
    highlightActiveMenuBasedOnUrl();

    // 2. Main Module Switcher Handler ✅ FIXED
    $(document).on('click', '.module-link', function (e) {
        e.preventDefault();
        $('.module-link').removeClass('active').css({
            'background': 'transparent',
            'border': 'none'
        }).addClass('text-white');

        $(this).addClass('active').css({
            'background': 'rgba(255, 255, 255, 0.15)',
            'border': '1px solid rgba(255, 255, 255, 0.2)'
        }).removeClass('text-white');

        let moduleName = $(this).data('module');
        let features = moduleFeatures[moduleName];

        if (features && features.length > 0) {
            // ✅ FIX 1: -1 → 0 (pehla feature active)
            renderSubMenu(moduleName, 0);
        }
    });

    // 3. Sub-menu Navigation Handler ✅ FIXED
    $(document).on('click', '#sub-menu-list .nav-link', function (e) {
        e.preventDefault();
        $('#sub-menu-list .nav-link').removeClass('active');
        $(this).addClass('active');

        let url = $(this).data('url');
        if (url) {
            // ✅ FIX 2: URL update + force highlight
            history.pushState(null, '', url);
            loadMainContent(url);
            highlightActiveMenuBasedOnUrl();
        }
    });

    // 4. Core Content Loader Function via AJAX GET
    function loadMainContent(url) {
        $('#main-content').html(`
            <div class="text-center py-5">
                <div class="spinner-border text-success" role="status" style="width: 3rem; height: 3rem;">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p class="text-muted mt-3 fw-medium">Loading module data...</p>
            </div>
        `);

        $.get(url)
            .done(function (data) {
                $('#main-content').html(data);
            })
            .fail(function (xhr) {
                if (xhr.status === 404) {
                    window.location.href = url;
                    return;
                }
                $('#main-content').html(`
                    <div class="alert alert-danger shadow-sm rounded-3 p-4">
                        <div class="d-flex align-items-center gap-3">
                            <i class="fa-solid fa-triangle-exclamation fa-2x text-danger"></i>
                            <div>
                                <h6 class="fw-bold mb-1">System Notice</h6>
                                <p class="mb-0 text-muted small">Failed to load content or endpoint not found (${url}).</p>
                            </div>
                        </div>
                    </div>
                `);
            });
    }

    // Helper: Render Sub-menu items dynamically
    function renderSubMenu(moduleName, activeIndex) {
        let features = moduleFeatures[moduleName];
        if (!features) return;

        let subMenuHtml = '';
        features.forEach(function (feature, index) {
            let activeClass = (index === activeIndex) ? 'active' : '';
            let iconClass = feature.icon ? `<i class="fa-solid ${feature.icon} me-1"></i>` : '';
            subMenuHtml += `
                <li class="nav-item">
                    <a class="nav-link btn-sm text-nowrap rounded-pill px-3 ${activeClass}" href="#" data-url="${feature.url}">
                        ${iconClass} ${feature.name}
                    </a>
                </li>
            `;
        });
        $('#sub-menu-list').html(subMenuHtml);
    }

    // Helper: Match current path to correct module and sub-feature
    function highlightActiveMenuBasedOnUrl() {
        let currentPath = window.location.pathname.toLowerCase();
        let foundModule = null;
        let foundIndex = 0;

        for (let mod in moduleFeatures) {
            let list = moduleFeatures[mod];
            for (let i = 0; i < list.length; i++) {
                if (currentPath.includes(list[i].url.toLowerCase())) {
                    foundModule = mod;
                    foundIndex = i;
                    break;
                }
            }
            if (foundModule) break;
        }

        if (!foundModule) {
            foundModule = "Inventory";
            foundIndex = 0;
        }

        $('.module-link').each(function () {
            if ($(this).data('module') === foundModule) {
                $(this).addClass('active').css({
                    'background': 'rgba(255, 255, 255, 0.15)',
                    'border': '1px solid rgba(255, 255, 255, 0.2)'
                }).removeClass('text-white');
            } else {
                $(this).removeClass('active').css({
                    'background': 'transparent',
                    'border': 'none'
                }).addClass('text-white');
            }
        });

        renderSubMenu(foundModule, foundIndex);
    }

    // 5. Open Modal via AJAX
    $(document).on('click', '.open-modal-btn', function (e) {
        e.preventDefault();
        let url = $(this).data('url');

        if (!url) return;

        $.get(url)
            .done(function (data) {
                $('#modal-content-body').html(data);
                let modalElement = document.getElementById('globalModal');
                if (modalElement) {
                    let myModal = new bootstrap.Modal(modalElement);
                    myModal.show();
                }
            })
            .fail(function () {
                showToast("Error loading form popup.", "danger");
            });
    });

    // 6. AJAX Form Submission Handler
    $(document).on('submit', '.ajax-form', function (e) {
        e.preventDefault();
        let form = $(this);

        $.ajax({
            type: form.attr('method') || 'POST',
            url: form.attr('action'),
            data: form.serialize(),
            success: function (response) {
                if (typeof response === 'object' && response.success) {
                    let modalElement = document.getElementById('globalModal');
                    if (modalElement) {
                        let modal = bootstrap.Modal.getInstance(modalElement);
                        if (modal) modal.hide();
                    }

                    let successPopupHtml = `
                        <div class="modal fade" id="successPopupModal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static">
                            <div class="modal-dialog modal-dialog-centered" style="max-width: 400px;">
                                <div class="modal-content border-0 shadow-lg rounded-4 text-center p-4">
                                    <div class="mb-3 mt-2">
                                        <div class="text-success display-4 mb-2">
                                            <i class="fas fa-check-circle"></i>
                                        </div>
                                        <h3 class="fw-bold text-dark">Success!</h3>
                                        <p class="text-secondary small mb-0 px-2">${response.message || "Operation completed successfully!"}</p>
                                    </div>
                                    <div class="d-grid mt-2">
                                        <button type="button" class="btn btn-success rounded-pill py-2 fw-semibold shadow-sm" id="successOkBtn">
                                            OK
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;

                    $('#successPopupModal').remove();
                    $('body').append(successPopupHtml);

                    let successModalEl = document.getElementById('successPopupModal');
                    let successModal = new bootstrap.Modal(successModalEl);
                    successModal.show();

                    let activeSubLink = $('#sub-menu-list .nav-link.active');
                    let currentUrl = response.redirectUrl || (activeSubLink.length > 0 ? activeSubLink.data('url') : window.location.pathname);

                    $(document).on('click', '#successOkBtn', function () {
                        successModal.hide();
                        $('#successPopupModal').remove();
                        if (currentUrl) {
                            history.pushState(null, '', currentUrl);
                            loadMainContent(currentUrl);
                        }
                    });

                } else {
                    $('#modal-content-body').html(response);
                }
            },
            error: function () {
                showToast("An unexpected error occurred.", "danger");
            }
        });
    });
});

// 7. Global Toast Notification System
function showToast(message, type = 'success') {
    let bgColors = {
        'success': 'bg-success text-white',
        'danger': 'bg-danger text-white',
        'warning': 'bg-warning text-dark',
        'info': 'bg-info text-dark'
    };

    let iconList = {
        'success': 'fa-circle-check',
        'danger': 'fa-circle-xmark',
        'warning': 'fa-triangle-exclamation',
        'info': 'fa-circle-info'
    };

    if ($('#toastContainer').length === 0) {
        $('body').append('<div id="toastContainer" class="toast-container position-fixed top-0 end-0 p-3" style="z-index: 1090; margin-top: 65px;"></div>');
    }

    let toastId = 'toast_' + Date.now();
    let toastHtml = `
        <div id="${toastId}" class="toast align-items-center ${bgColors[type] || 'bg-primary text-white'} border-0 shadow-lg mb-2 rounded-3" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex p-2">
                <div class="toast-body d-flex align-items-center gap-2 fw-semibold">
                    <i class="fa-solid ${iconList[type] || 'fa-bell'} fa-lg"></i>
                    <span>${message}</span>
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        </div>
    `;

    $('#toastContainer').append(toastHtml);
    let toastElement = document.getElementById(toastId);
    let toast = new bootstrap.Toast(toastElement, { delay: 4000 });
    toast.show();

    toastElement.addEventListener('hidden.bs.toast', function () {
        $(this).remove();
    });
}