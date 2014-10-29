﻿using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace DryIoc.CommonServiceLocator.UnitTests
{
    public class DryIocServiceLocatorTests
    {
        [Test]
        public void Can_get_instance_with_locator()
        {
            var locator = new DryIocServiceLocator(new Container());
            locator.Container.Register<IClient, Client>();
            locator.Container.Register<IService, Service>();

            var client = locator.GetInstance<IClient>();

            Assert.That(client.Dependency, Is.InstanceOf<Service>());
        }

        [Test]
        public void Can_get_keyyed_instance_with_locator()
        {
            var locator = new DryIocServiceLocator(new Container());
            locator.Container.Register<IService, Service>(named: "1");

            var service = locator.GetInstance<IService>("1");

            Assert.That(service, Is.InstanceOf<Service>());
        }

        [Test]
        public void Can_get_all_instances_with_locator()
        {
            var locator = new DryIocServiceLocator(new Container());
            locator.Container.Register<IService, Service>();
            locator.Container.Register<IService, AnotherService>(named: "another");

            var services = locator.GetAllInstances<IService>().ToArray();

            Assert.That(services[0], Is.InstanceOf<Service>());
            Assert.That(services[1], Is.InstanceOf<AnotherService>());
        }

        [Test]
        public void Get_instance_of_not_registered_service_should_Throw()
        {
            var locator = new DryIocServiceLocator(new Container());
            locator.Container.Register<IClient, Client>();

            Assert.Throws<ActivationException>(() =>
                locator.GetInstance<IClient>());
        }

        [Test]
        public void Get_all_instances_of_not_registered_service_should_return_empty_collection()
        {
            var locator = new DryIocServiceLocator(new Container());
            locator.Container.Register<IClient, Client>();

            var clients = locator.GetAllInstances<IClient>().ToArray();

            Assert.That(clients.Length, Is.EqualTo(0));
        }

        public interface IService { }
        public class Service : IService { }
        public class AnotherService : IService { }

        public interface IClient
        {
            IService Dependency { get; set; }
        }

        public class Client : IClient
        {
            public IService Dependency { get; set; }

            public Client(IService dependency)
            {
                Dependency = dependency;
            }
        }
    }
}
