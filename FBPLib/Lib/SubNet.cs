using System;
using System.Collections.Generic;
using System.Text;
using Spring.Threading.Helpers;

namespace FBPLib
{
    public class SubNet : Network
    {
        /* *
         * Copyright 2007, 2008, 2009, J. Paul Morrison.  At your option, you may copy, 
         * distribute, or make derivative works under the terms of the Clarified Artistic License, 
         * based on the Everything Development Company's Artistic License.  A document describing 
         * this License may be found at http://www.jpaulmorrison.com/fbp/artistic2.htm. 
         * THERE IS NO WARRANTY; USE THIS PRODUCT AT YOUR OWN RISK.
         * */

         public SubNet()
         {
            InitBlock();
                       
         }
        public override void Define()
        {
        }

        public override void Execute()
        {
            OutputPort subEndPort = null;
            
            if (_status != States.Error)
            {
                //_network.Trace(this.Name + ": started");
                _components.Clear();
                //_tracing = _mother._tracing;                
                //_traceFileList = _mother._traceFileList;
                subEndPort = _outputPorts["*SUBEND"];

                try
                {
                    CallDefine();
                    bool res = true;
                    foreach (Component comp in _components.Values)
                    {
                        res &= comp.CheckPorts();
                    }
                    if (!res)
      	                 FlowError.Complain("One or more mandatory connections have been left unconnected: " + Name);
                    _cdl = new CountDownLatch(_components.Count);

                    Initiate();
                    // activateAll();
                    // don't do deadlock testing in subnets - you need to consider the whole net!
                    _deadlockTest = false;
                    WaitForAll();

                    foreach (IInputPort ip in _inputPorts.Values)
                        if (ip is InitializationConnection)
                        {
                            InitializationConnection ic = (InitializationConnection)ip;
                            ic.Close();
                        }

                    /*
                     * Iterator allout = (outputPorts.values()).iterator();
                     * 
                     * while (allout.hasNext()) {
                     * 
                     * 
                     * OutputPort op = (OutputPort) allout.next(); op.close();
                     * 
                     *  }
                     */
                    // status = Component.StatusValues.TERMINATED; //will not be set if
                    // never activated
                    // mother.indicateTerminated(this);
                    _network.Trace(this.Name + ": closed down");
                    if (subEndPort != null)
                    {
                        subEndPort.Send(Create(null));
                    }
                }
                catch (FlowError e)
                {
                    string s = "Flow Error :" + e;
                    Console.Out.WriteLine("Network: " + s);
                    throw e;
                }
            }
        }

        public override void OpenPorts()
        {
        }
        public override void SignalError(Exception e)
        {
            if (_status != States.Error)
            {
                _mother.SignalError(e);
                Terminate(States.Error);
            }
        }
       // public override Object[] Introspect()
      //  {
       //     return new Object[] {
		//"manages a subnet"};
     //   }
        internal override void Terminate(States newStatus)
        {
            foreach (Component comp in _components.Values)
            {
                comp.Terminate(newStatus);
            }
            _status = newStatus;
            _thread.Interrupt();
        }
        /* The next two methods were imported unchanged from Java - they will
         * be rewritten when the need arises

        ***
   * Declares input ports not specified in annotations.
   * @param portName the name of the input port
   **
  protected void DeclareInputPort(String portName) {
    inputPortAttrs.put(portName, new InPort() {

      public boolean arrayPort() {
        return false;
      }

      public String description() {
        return "";
      }

      public Class type() {
        return Object.class;
      }

      public String value() {
        return portName;
      }

      public Class<? extends Annotation> annotationType() {
        return this.getClass();
      }
    });
  }

  ***
   * Declares output ports not specified in annotations.
   * @param portName the name of the output port
   **
  protected void declareOutputPort(final String portName) {
    outputPortAttrs.put(portName, new OutPort() {

      public boolean arrayPort() {
        return false;
      }

      public String description() {
        return "";
      }

      public Class type() {
        return Object.class;
      }

      public String value() {
        return portName;
      }

      public Class<? extends Annotation> annotationType() {
        return this.getClass();
      }

      public boolean optional() {
        return false;
      }
    });
  }
*/


    }
}
