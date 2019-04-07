using GlobalProductCorp.Models;
using GlobalProductCorp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalProductCorp.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private GlobalProductCorpContext db = new GlobalProductCorpContext();

        // GET: Orders
        public ActionResult NewOrder()
        {
            var orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();

            // Creamos el orderView en sesión para recuperarlo
            Session["orderView"] = orderView;

            // Armamos listado con todos los clientes
            var list = db.Customers.ToList();
            list.Add(new Customer { CustomerID = 0, FirstName = "[ Seleccione un Cliente... ]" });
            list = list.OrderBy(c => c.FullName).ToList();

            // Generamos ViewBag
            ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");

            return View(orderView);
        }


        // POST: Orders
        [HttpPost]
        public ActionResult NewOrder(OrderView orderView)
        {
            // Recuperamos el bojeto de la sesión
            orderView = Session["orderView"] as OrderView;

            // Capturamos el Cliente ingresado
            var customerID = int.Parse(Request["CustomerID"]);

            // Validamos que haya ingresado Cliente
            if (customerID == 0)
            {
                // Armamos listado con todos los clientes
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[ Seleccione un Cliente... ]" });
                list = list.OrderBy(c => c.FullName).ToList();

                // Generamos ViewBag
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                // Armamos el viewbag con los errores contenidos
                ViewBag.Error = "Debe seleccionar un Cliente";

                return View(orderView);
            }

            // Validamos que producto seleccionado exista
            var customer = db.Customers.Find(customerID);
            if (customer == null)
            {
                // Armamos listado con todos los clientes
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[ Seleccione un Cliente... ]" });
                list = list.OrderBy(c => c.FullName).ToList();

                // Generamos ViewBag
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                // Armamos el viewbag con los errores contenidos
                ViewBag.Error = "Cliente seleccionado no existe";

                return View(orderView);
            }

            if (orderView.Products.Count == 0)
            {
                // Armamos listado con todos los clientes
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[ Seleccione un Cliente... ]" });
                list = list.OrderBy(c => c.FullName).ToList();

                // Generamos ViewBag
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                // Armamos el viewbag con los errores contenidos
                ViewBag.Error = "Debe ingresar productos";

                return View(orderView);
            }

            // Generamos variable para orderID
            int orderID = 0;

            // Realizamos transaccionlidad
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                { 
                    // Generamos la orden
                    var order = new Order
                    {
                        CustomerID = customerID,
                        DateOrder = DateTime.Now
                    };

                    // Llevamos la orden a la BD
                    db.Orders.Add(order);
                    db.SaveChanges();

                    // Obtenemos el ID de la orden grabada
                    orderID = db.Orders.ToList().Select(o => o.OrderID).Max();

                    // Grabamos los productos de de la orden
                    foreach (var item in orderView.Products)
                    {
                        var orderDetail = new OrderDetail
                        {
                            ProductID = item.ProductID,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            OrderID = orderID,
                            IVA = 19
                        };

                        db.OrderDetails.Add(orderDetail);
                        db.SaveChanges();
                    }

                    // Confirmamos la transacción si no hubo error
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error en Base de Datos: {0}" + ex.Message;

                    // Armamos listado con todos los clientes
                    var listCu = db.Customers.ToList();
                    listCu.Add(new Customer { CustomerID = 0, FirstName = "[ Seleccione un Cliente... ]" });
                    listCu = listCu.OrderBy(c => c.FullName).ToList();

                    // Generamos ViewBag
                    ViewBag.CustomerID = new SelectList(listCu, "CustomerID", "FullName");

                    return View(orderView);
                }
            }

            ViewBag.Message = string.Format("La orden {0} ha sido grabada exitosamente.", orderID);

            // Armamos listado con todos los clientes
            var listC = db.Customers.ToList();
            listC.Add(new Customer { CustomerID = 0, FirstName = "[ Seleccione un Cliente... ]" });
            listC = listC.OrderBy(c => c.FullName).ToList();

            // Generamos ViewBag
            ViewBag.CustomerID = new SelectList(listC, "CustomerID", "FullName");

            // Reestablecemos valores de la vista
            orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();

            // Creamos el orderView en sesión para recuperarlo
            Session["orderView"] = orderView;

            return View(orderView);
        }

        // GET: Agregar producto a la Orden actual
        public ActionResult AddProduct()
        {
            // Armamos listado con todos los productos
            var list = db.Products.ToList();
            list.Add(new ProductOrder { ProductID = 0, Description = "[ Seleccione un Producto... ]" });
            list = list.OrderBy(p => p.Description).ToList();

            // Generamos ViewBag
            ViewBag.ProductID = new SelectList(list, "ProductID", "Description");

            return View();
        }

        // POST: Agregar producto a la Orden actual
        [HttpPost]
        public ActionResult AddProduct(ProductOrder productOrder)
        {
            // Variable para enviar error si ocurre
            var Error = "";

            // Recuperamos el bojeto de la sesión
            var orderView = Session["orderView"] as OrderView;

            // Validar si usuario envió producto y cantidad correcta
            var productID = int.Parse(Request["ProductID"]);
            var quantity = Request["Quantity"];
            var flag = false;

            // Validmos que haya ingresado producto
            if (productID == 0)
            {
                flag = true;
                Error += "Debe seleccionar un Producto. \n";
            }
            // Validamos cantidad ingresada
            if (quantity == null || quantity == "" || int.Parse(quantity) < 1)
            {
                flag = true;
            }

            // Si hubo error
            if (flag)
            {
                // Armamos listado con todos los productos
                var list = db.Products.ToList();
                list.Add(new ProductOrder { ProductID = 0, Description = "[ Seleccione un Producto... ]" });
                list = list.OrderBy(p => p.Description).ToList();

                // Generamos ViewBag con el listado de productos
                ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
                // Armamos el viewbag con los errores contenidos
                ViewBag.Error = Error;

                return View(productOrder);
            }

            // Validamos que producto seleccionado exista
            var product = db.Products.Find(productID);
            if (product == null)
            {
                // Armamos listado con todos los productos
                var list = db.Products.ToList();
                list.Add(new ProductOrder { ProductID = 0, Description = "[ Seleccione un Producto... ]" });
                list = list.OrderBy(p => p.Description).ToList();

                // Generamos ViewBag
                ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
                ViewBag.Error = "Producto seleccionado no existe.";

                return View(productOrder);
            }

            // Validamos si producto seleccionado ya existe en la lista de la orden
            productOrder = orderView.Products.Find(p => p.ProductID == productID);

            if (productOrder == null)
            {
                // Validamos la cantidad disponible en inventario
                int stock = product.Stock;
                if(int.Parse(Request["Quantity"]) > stock)
                {
                    // Armamos listado con todos los productos
                    var list = db.Products.ToList();
                    list.Add(new ProductOrder { ProductID = 0, Description = "[ Seleccione un Producto... ]" });
                    list = list.OrderBy(p => p.Description).ToList();

                    // Generamos ViewBag
                    ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
                    ViewBag.Error = "No hay suficientes unidades del producto. Disponible(s) " + stock + " en stock";

                    return View(productOrder);
                }

                // Si producto existe y todo está bien, lo agregamos
                productOrder = new ProductOrder
                {
                    ProductID = product.ProductID,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = int.Parse(Request["Quantity"])
                };

                // Adicionamos los cambios a la lista de la orden
                orderView.Products.Add(productOrder);
            }
            else
            {
                // Validamos la cantidad disponible en inventario
                int stock = product.Stock - productOrder.Quantity;
                if (int.Parse(Request["Quantity"]) > stock)
                {
                    // Armamos listado con todos los productos
                    var list = db.Products.ToList();
                    list.Add(new ProductOrder { ProductID = 0, Description = "[ Seleccione un Producto... ]" });
                    list = list.OrderBy(p => p.Description).ToList();

                    // Generamos ViewBag
                    ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
                    ViewBag.Error = "No hay suficientes unidades del producto. Disponible(s) " + stock + " en stock";

                    return View(productOrder);
                }

                productOrder.Quantity += int.Parse(Request["Quantity"]);
            }

            // Armamos listado con todos los clientes
            var listC = db.Customers.ToList();
            listC.Add(new Customer { CustomerID = 0, FirstName = "[ Seleccione un Cliente... ]" });
            listC = listC.OrderBy(c => c.FullName).ToList();

            // Generamos ViewBag
            ViewBag.CustomerID = new SelectList(listC, "CustomerID", "FullName");

            return View("NewOrder", orderView);
        }

        // Método para cancelar cuando se agrega un nuevo producto a la orden
        public ActionResult BackToOrder()
        {
            // Recuperamos el bojeto de la sesión
            var orderView = Session["orderView"] as OrderView;

            // Armamos listado con todos los clientes
            var listC = db.Customers.ToList();
            listC.Add(new Customer { CustomerID = 0, FirstName = "[ Seleccione un Cliente... ]" });
            listC = listC.OrderBy(c => c.FullName).ToList();

            // Generamos ViewBag
            ViewBag.CustomerID = new SelectList(listC, "CustomerID", "FullName");

            return View("NewOrder", orderView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}